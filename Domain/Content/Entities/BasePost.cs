using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Content.Entities
{
    public abstract class BasePost
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public string Title { get; protected set; }
        public string AuthorId { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        protected BasePost(string title, string authorId)
        {
            Title = title;
            AuthorId = authorId;
        }

        // Hver posttype skal selv give en kort visning/summary
        public abstract string GetSummary();
    }
}

