using Application;
using Application.Abstractions.Contracts.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Gateways
{
    public class PythonChatGateway : IChatService
    {
        private readonly HttpClient _http;

        public PythonChatGateway(HttpClient http)
        {
            _http = http;
            // BaseAddress bliver sat i ServiceCollectionExtensions, så vi skal ikke sætte den her igen
        }

        public async Task<string> SendChatAsync(string role, string message)
        {
            var request = new { role, message };
            var response = await _http.PostAsJsonAsync("/chat", request);

            if (!response.IsSuccessStatusCode)
                return $"Fejl ved AI-microservice: {response.StatusCode}";

            var result = await response.Content.ReadFromJsonAsync<ChatResponse>();
            return result?.response ?? "(tomt svar)";
        }

        private class ChatResponse
        {
            public string response { get; set; }
        }
    }
}
