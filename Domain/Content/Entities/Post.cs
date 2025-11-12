using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content.Events;
using Domain.Shared;

namespace Domain.Content.Entities
{
    /// <summary>
    /// Repræsenterer et indlæg (blog / health tip) i systemet.
    /// </summary>
    public class Post
    {
        // === Properties (felter i databasen) ===
        public Guid Id { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;  // fx Blog | HealthTip
        public bool IsPaywalled { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // === Domænehændelser ===
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // === Konstruktorer ===
        private Post() { } // til EF Core

        public Post(Guid authorId, string title, string body, string category, bool isPaywalled)
        {
            Id = Guid.NewGuid();
            AuthorId = authorId;
            Title = title;
            Body = body;
            Category = category;
            IsPaywalled = isPaywalled;
            CreatedAt = DateTime.UtcNow;
        }

        // === Domæne-adfærd ===
        public void Publish()
        {
            if (IsPublished)
                return; // allerede udgivet

            IsPublished = true;

            // Rejs et DDD-event (internt signal til systemet)
            _domainEvents.Add(new PostPublishedEvent(Id, Title));
        }

        public void ClearDomainEvents() => _domainEvents.Clear();

        // (Evt. flere metoder, fx UpdateBody, Unpublish osv.)
    }
}

