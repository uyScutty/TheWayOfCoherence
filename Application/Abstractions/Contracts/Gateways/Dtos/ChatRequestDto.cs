using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Contracts.Gateways.Dtos
{
    public class ChatRequestDto
    {
        public string Role { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
