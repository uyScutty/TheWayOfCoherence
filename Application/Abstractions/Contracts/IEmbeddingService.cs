using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Application.Abstractions.Contracts
{
    public interface IEmbeddingService
    {
        Task<float[]> CreateEmbeddingAsync(string text);
    }

    // Simple stub embedding service (replace with OpenAI or anden embedder)
    public class StubEmbeddingService : IEmbeddingService
    {
        public Task<float[]> CreateEmbeddingAsync(string text)
        {
            // Dummy embedding: zero vector
            return Task.FromResult(new float[1536]); // adjust embedding size if needed
        }
    }

    public class VectorDbService
    {
        private readonly HttpClient _http;
        private readonly IEmbeddingService _embeddings;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public VectorDbService(IHttpClientFactory factory, IEmbeddingService embeddings)
        {
            _http = factory.CreateClient("chroma");
            _embeddings = embeddings;
        }

        // Ensure collection exists (v2 API: POST /collections)
        public async Task EnsureCollectionAsync(string collectionName)
        {
            var payload = new { name = collectionName };
            var resp = await _http.PostAsJsonAsync($"/collections/{collectionName}/points", payload);

            // 409 = collection already exists
            if (!resp.IsSuccessStatusCode && resp.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                var body = await resp.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to create collection: {resp.StatusCode} {body}");
            }
        }

        // Add document with embedding
        public async Task AddDocumentAsync(string collectionName, string id, string text, Dictionary<string, string>? metadata = null)
        {
            var embedding = await _embeddings.CreateEmbeddingAsync(text);

            var payload = new
            {
                ids = new[] { id },
                embeddings = new[] { embedding },
                metadatas = new[] { metadata ?? new Dictionary<string, string> { { "text", text } } },
                documents = new[] { text }
            };

            var resp = await _http.PostAsJsonAsync($"/api/v2/collections/{collectionName}/points", payload);
            resp.EnsureSuccessStatusCode();
        }

        // Query by text
        public async Task<List<(string Id, string Text, Dictionary<string, string> Metadata)>> QueryByTextAsync(string collectionName, string queryText, int topK = 5)
        {
            var embedding = await _embeddings.CreateEmbeddingAsync(queryText);
            return await QueryByEmbeddingAsync(collectionName, embedding, topK);
        }

        // Query by embedding
        public async Task<List<(string Id, string Text, Dictionary<string, string> Metadata)>> QueryByEmbeddingAsync(string collectionName, float[] queryEmbedding, int topK = 5)
        {
            var payload = new
            {
                query_embeddings = new[] { queryEmbedding },
                n_results = topK
            };

            var resp = await _http.PostAsJsonAsync($"/api/v2/collections/{collectionName}/query", payload);
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync();
            var doc = await JsonDocument.ParseAsync(stream);
            var root = doc.RootElement;

            var results = new List<(string Id, string Text, Dictionary<string, string> Metadata)>();

            if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
            {
                foreach (var resItem in resultsArray.EnumerateArray())
                {
                    if (!resItem.TryGetProperty("ids", out var ids)) continue;
                    if (!resItem.TryGetProperty("metadatas", out var metas)) continue;

                    var idsEnum = ids.EnumerateArray();
                    var metasEnum = metas.EnumerateArray();

                    foreach (var pair in idsEnum.Zip(metasEnum))
                    {
                        string id = pair.First.GetString() ?? "";
                        var metaDict = new Dictionary<string, string>();
                        string text = "";

                        if (pair.Second.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var prop in pair.Second.EnumerateObject())
                                metaDict[prop.Name] = prop.Value.GetString() ?? "";

                            if (metaDict.TryGetValue("text", out var t)) text = t;
                        }

                        results.Add((id, text, metaDict));
                    }
                }
            }

            return results;
        }
    }
}
