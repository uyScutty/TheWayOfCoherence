using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Contracts.Gateways
{
    public interface IChatService
    {
        Task<string> SendChatAsync(string role, string message);
    }
}
