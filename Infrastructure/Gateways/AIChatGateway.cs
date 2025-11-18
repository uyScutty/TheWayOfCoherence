using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Contracts.Gateways;
using Application.Abstractions.Contracts.Gateways.Dtos;

namespace Infrastructure.Gateways
{
    public class AIChatGateway : IAIChatGateway
    {
        private readonly HttpClient _httpClient;

        public AIChatGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChatResponseDto> SendMessageAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("/chat", request, cancellationToken);
            httpResponse.EnsureSuccessStatusCode(); // smid exception hvis fejl
            return await httpResponse.Content.ReadFromJsonAsync<ChatResponseDto>(cancellationToken: cancellationToken)
                   ?? new ChatResponseDto { Response = "Ingen svar fra AI" };
        }
    }
}
