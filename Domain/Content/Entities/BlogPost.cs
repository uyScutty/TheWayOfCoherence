using Domain.Content.Events;
using Domain.Shared;

namespace Domain.Content.Entities
{
    public class BlogPost : BasePost
    {
        public string? ImageUrl { get; private set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private BlogPost() { }

        public BlogPost(Guid authorId, string title, string body, string? imageUrl = null)
            : base(authorId, title, body)
        {
            ImageUrl = imageUrl;
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


