using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.Features.UserProfiles.Dtos;
using MediatR;
using Application.Features.UserProfiles.Interfaces;

namespace Application.Features.UserProfiles.Queries
{
    public class ListProfilesQueryHandler : IRequestHandler<ListProfilesQuery, IEnumerable<UserProfileDto>>
    {
        private readonly IUserProfileRepository _repo;
        public ListProfilesQueryHandler(IUserProfileRepository repo)
            => _repo = repo;
        public async Task<IEnumerable<UserProfileDto>> Handle(
            ListProfilesQuery query,
            CancellationToken ct)
        {
            var profiles = await _repo.ListAsync(ct);
            return profiles.Select(static p => new UserProfileDto(p.Id,p.UserId, p.Age, p.Gender, p.
                HealthNote)

            );
        }
    }
}
