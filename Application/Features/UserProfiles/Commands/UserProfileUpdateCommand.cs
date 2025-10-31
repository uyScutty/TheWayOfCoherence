using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;

namespace Application.Features.UserProfiles.Commands
{
    public record UserProfileUpdateCommand : IRequest
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Age { get; init; }
        public string Gender { get; init; }
        public string HealthNote { get; init; }


    }
}

