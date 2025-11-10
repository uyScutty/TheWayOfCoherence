using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Posts.Contracts;
using Domain.Content;

namespace Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _db;

        public PostRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Post?> GetByIdAsync(Guid id)
            => await _db.Posts.FindAsync(id);

        public async Task UpdateAsync(Post post)
        {
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
        }
    }
}
