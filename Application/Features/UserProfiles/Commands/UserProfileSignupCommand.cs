using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.UserProfiles.Commands
{
    public sealed record UserProfileSignupCommand(
        Guid UserId,
        string Age,
        string Gender,
        string HealthNote) : IRequest<Guid>;
}
