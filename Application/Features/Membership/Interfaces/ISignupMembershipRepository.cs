using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Membership.Interfaces;

public interface ISignupMembershipRepository
{
    Task SaveMemberAsync(string fullName);
}
