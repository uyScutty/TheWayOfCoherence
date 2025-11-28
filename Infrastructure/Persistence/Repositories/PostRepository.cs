using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Posts.Contracts;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Posts.FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post.Id;
        }

        public async Task<List<Post>> ListAsync(bool? isPublished = null, string? category = null)
        {
            var query = _context.Posts.AsQueryable();

            if (isPublished.HasValue)
            {
                query = query.Where(p => p.IsPublished == isPublished.Value);
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}

