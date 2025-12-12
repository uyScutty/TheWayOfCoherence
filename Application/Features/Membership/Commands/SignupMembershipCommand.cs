using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Membership.Commands
{
    public sealed record SignupMembershipCommand(Guid UserId) : IRequest<Guid>;
}
