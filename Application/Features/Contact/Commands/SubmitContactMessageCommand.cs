using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Contact.Commands
{
    public record SubmitContactMessageCommand(
        string Name,
        string Email,
        string Subject,
        string Message
    ) : IRequest;

}
