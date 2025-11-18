using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Contracts
{
    public interface IUserLookupService
    {
        Task<string> GetEmailByUserIdAsync(Guid userId, CancellationToken ct);
    }
}