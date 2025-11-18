using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class UserLookupService : IUserLookupService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserLookupService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetEmailByUserIdAsync(Guid userId, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user?.Email ?? "";
        }
    }
}
