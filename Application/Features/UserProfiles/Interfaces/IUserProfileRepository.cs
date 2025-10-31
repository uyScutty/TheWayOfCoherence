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
        Task<UserProfile> GetByIdAsync(UserProfile userId, CancellationToken ct);

        Task AddAsync(UserProfile profile, CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);

        Task DeleteChangesAsync(UserProfile profile, CancellationToken ct);

        Task<UserProfile> ListAsync(CancellationToken ct);

        Task UpdateChangesAsync(UserProfile profile, CancellationToken ct);
    }
}
