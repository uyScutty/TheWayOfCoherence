using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.UserProfiles.Commands;
using Application.Features.UserProfiles.Interfaces;
using Application.Features.UserProfiles.Interfaces;
using Domain.Users;
using MediatR;

namespace Application.Features.UserProfiles.Handlers
{
    public class UserProfileSignupHandler : IRequestHandler<UserProfileSignupCommand, Guid>
    {
        private readonly IUserProfileRepository _repo;

        public UserProfileSignupHandler(IUserProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(UserProfileSignupCommand cmd, CancellationToken ct)
        {
            var profile = new UserProfile(
                cmd.UserId,
                cmd.Age,
                cmd.Gender,
                cmd.HealthNote
            );

            await _repo.AddAsync(profile, ct);
            await _repo.SaveChangesAsync(ct);

            return profile.Id;
        }
    }
}

