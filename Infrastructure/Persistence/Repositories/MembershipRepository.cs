using Application.Abstractions.Contracts;
using Application.Features.Membership.Dtos;
using Application.Features.Membership.Interfaces;
using Domain.Membership;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly AppDbContext _db;
        private readonly IUserLookupService _userLookup;

        public MembershipRepository(AppDbContext db, IUserLookupService userLookup)
        {
            _db = db;
            _userLookup = userLookup;
        }

        public async Task<IEnumerable<MemberDto>> GetActiveMembersAsync(CancellationToken ct)
        {
            var active = await _db.Memberships
                .Where(m => m.IsActive)
                .ToListAsync(ct);

            var list = new List<MemberDto>();

            foreach (var m in active)
            {
                var email = await _userLookup.GetEmailByUserIdAsync(m.UserId, ct);

                list.Add(new MemberDto
                {
                    UserId = m.UserId,
                    Email = email
                });
            }

            return list;
        }

        public async Task CreateAsync(MembershipUser membership, CancellationToken ct)
        {
            await _db.Memberships.AddAsync(membership, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}





