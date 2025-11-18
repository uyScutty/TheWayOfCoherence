using Application.Features.Membership.Dtos;
using Domain.Membership;

namespace Application.Features.Membership.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<MemberDto>> GetActiveMembersAsync(CancellationToken ct = default);
        Task CreateAsync(MembershipUser membership, CancellationToken ct);
    }
}
