using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Domain;

namespace Application.Features.Membership.Commands
{
    public record SignupMembershipCommand : IRequest
    {
        Guid Id;
        Guid UserId;

        bool IsActive;
        DateTime CreatedAT;

    } 
}
