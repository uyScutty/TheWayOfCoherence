using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.UserProfiles.Commands
{
    public record UserProfileSignupCommand : IRequest<Guid>
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Age { get; init; }
        public string Gender { get; init; }
        public string HealthNote { get; init; }

      
    }
    }
