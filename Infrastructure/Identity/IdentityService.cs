using Application.Abstractions.Contracts;
using Application.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> CreateUserAsync(string email, string password, string fullName, CancellationToken ct)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = email,
                FullName = fullName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(
                    string.Join(", ", result.Errors.Select(x => x.Description))
                );

            return user.Id;
        }
    }
}
