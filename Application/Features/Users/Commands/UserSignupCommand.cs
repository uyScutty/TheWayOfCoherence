using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Users.Commands
{
    public record UserSignupCommand(string Email, string Password, string FullName)
        : IRequest<Guid>;
}
