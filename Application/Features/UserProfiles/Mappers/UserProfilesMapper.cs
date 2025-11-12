using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.UserProfiles.Dtos;
using Domain.UserProfile;

namespace Application.Features.UserProfiles.Mappers
{
    public static class UserProfileMapper
    {
        public static UserProfileDto ToDto(UserProfile entity)
        {
            return new UserProfileDto(
                entity.Id,
                entity.UserId,
                entity.Age,
                entity.Gender,
                entity.HealthNote
            );
        }
    }
}

