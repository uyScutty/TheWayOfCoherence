using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content;

namespace Application.Features.Posts.Contracts
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(Guid id);
        Task UpdateAsync(Post post);
    }
}
