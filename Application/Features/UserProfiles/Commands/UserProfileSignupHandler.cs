using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.UserProfile;
using Application.Features.UserProfiles.Interfaces;
using MediatR;
using Application.Features.UserProfiles.Commands;


namespace Application.Features.UserProfiles.Commands
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
                cmd.HealthNote // Removed the trailing comma here
            );
            await _repo.AddAsync(profile, ct); // This line now references the correctly declared 'profile'
            await _repo.SaveChangesAsync(ct);
            return profile.Id;
        }

     
    }
}
