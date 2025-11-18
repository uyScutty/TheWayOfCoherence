using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Contracts
{
    public interface IIdentityService
    {
        Task<Guid> CreateUserAsync(string email, string password, string fullName, CancellationToken ct);
    }
}
