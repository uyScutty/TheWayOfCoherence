using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime.Internal.UserAgent;
using Application.Features.UserProfiles.Interfaces;
using Domain.UserProfile;
using Domain.Users;
using MediatR;

namespace Application.Features.UserProfiles.Commands
{
    public class UserProfileUpdateHandler : IRequestHandler<UserProfileUpdateCommand, Guid>
    {
        private readonly IUserProfileRepository _repo;

        public UserProfileUpdateHandler(IUserProfileRepository repo)
        {
            _repo = repo;
        }
        public async Task<Guid> Handle(UserProfileUpdateCommand cmd, CancellationToken ct)
        {
            var profile = await GetByIdAsync(UserProfile userProfile, CancellationToken ct)
                 cmd.UserId = UserProfile
                 cmd = profile.userID,
                 cmd.
                 cmd.Gender,
                 cmd.HealthNote // Removed the trailing comma here
        };
        await _repo.Update(profile, ct); // This line now references the correctly declared 'profile'
        await _repo.SaveChangesAsync(ct);
            return profile.Id;

            




    }
}
