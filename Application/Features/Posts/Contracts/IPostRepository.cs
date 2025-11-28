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
        Task<Post?> GetByIdAsync(Guid id);
        Task UpdateAsync(Post post);
        Task<Guid> AddAsync(Post post);
        Task<List<Post>> ListAsync(bool? isPublished = null, string? category = null);
    }
}
