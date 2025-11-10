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
        public string Body { get; private set; }
        public string? ImageUrl { get; private set; }

        private readonly List<object> _domainEvents = new(); // Fixed syntax error: removed extra '>'.

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        public BlogPost(string title, string authorId, string body, string? imageUrl = null)
            : base(title, authorId)
        {
            Body = body;
            ImageUrl = imageUrl;
        }

        public override string GetSummary() =>
           Body.Length > 120 ? Body[..120] + "..." : Body;

        public void Publish()
        {
            _domainEvents.Add(new PostPublishedEvent(Id, Title));
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}

