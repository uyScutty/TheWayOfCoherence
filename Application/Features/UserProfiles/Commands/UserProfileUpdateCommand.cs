using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Features.UserProfiles.Dtos;
using MediatR;

namespace Application.Features.UserProfiles.Commands
{
    public record UserProfileUpdateCommand : IRequest<UserProfileDto>
    {
        
        public Guid UserId { get; init; }
      
        public string Gender { get; init; }
        public string HealthNote { get; init; }


    }
}

