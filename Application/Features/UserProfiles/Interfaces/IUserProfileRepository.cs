using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.UserProfile;

namespace Application.Features.UserProfiles.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(Guid userId, CancellationToken ct);
        Task<IEnumerable<UserProfile>> ListAsync(CancellationToken ct);
        Task AddAsync(UserProfile profile, CancellationToken ct);
        Task UpdateAsync(UserProfile profile, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}

