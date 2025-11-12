using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content.Events;
using Domain.Shared;

namespace Domain.Content.Entities
{
    public class BlogPost : BasePost, IPublishable
    {
       
        public string? ImageUrl { get; private set; }

        private readonly List<object> _domainEvents = new(); // Fixed syntax error: removed extra '>'.

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        private BlogPost() { }

        public BlogPost(Guid authorId, string title, string body, string? imageUrl = null)
            : base(authorId, title, body)  // 👈 først kald til base
        {
            ImageUrl = imageUrl;           // 👈 derefter egne felter
        }

        public override string GetSummary()
            => $"{Title} ({CreatedAt:dd/MM/yyyy})";
    

        public void Publish()
        {
            _domainEvents.Add(new PostPublishedEvent(Id, Title));
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}

