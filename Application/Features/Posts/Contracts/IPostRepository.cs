using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content;
using Domain.Content.Entities;

namespace Application.Features.Posts.Contracts
{
    public interface IPostRepository
    {
        Task<BasePost?> GetByIdAsync(Guid id);
        Task UpdateAsync(BasePost post);
    }
}
