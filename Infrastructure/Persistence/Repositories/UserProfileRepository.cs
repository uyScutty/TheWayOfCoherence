using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.UserProfiles.Interfaces;
using Domain.UserProfile;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId, ct);
        }

        public async Task<IEnumerable<UserProfile>> ListAsync(CancellationToken ct)
        {
            return await _context.UserProfiles.ToListAsync(ct);
        }

        public async Task AddAsync(UserProfile profile, CancellationToken ct)
        {
            await _context.UserProfiles.AddAsync(profile, ct);
        }

        public async Task UpdateAsync(UserProfile profile, CancellationToken ct)
        {
            _context.UserProfiles.Update(profile);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
