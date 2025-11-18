using Application.Abstractions.Contracts.Gateways.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Contracts.Gateways
{
    public interface IAIChatGateway
    {
        Task<ChatResponseDto> SendMessageAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
