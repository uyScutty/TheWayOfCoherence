using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Application.Features.UserProfiles.Dtos;

namespace Application.Features.UserProfiles.Queries
{
    public record ListProfilesQuery() : IRequest<IEnumerable<UserProfileDto>>
    {

    }
}
