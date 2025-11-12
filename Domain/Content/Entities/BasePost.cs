using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content.Events;
using Domain.Shared;

namespace Domain.Content.Entities
{
    public abstract class BasePost
    {

        private readonly List<IDomainEvent> _domainEvents = new();
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public string AuthorId { get; protected set; }
        public string Title { get; protected set; }

        public string Body { get; protected set; } = string.Empty;
     

        public bool IsPublished { get; protected set; }

        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


        protected BasePost() { } // For EF Core

        protected BasePost(Guid authorId, string title, string body)
        {
            Id = Guid.NewGuid();
            AuthorId = authorId.ToString(); // Convert Guid to string
            Title = title;
            Body = body;
            CreatedAt = DateTime.UtcNow;
         
        }



        public virtual void Publish()
        {
            if (IsPublished)
                throw new InvalidOperationException("Post is already published.");

            IsPublished = true;

            // Rejs et domain event
            AddDomainEvent(new Events.PostPublishedEvent(Id, Title));
        }
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        // Hver posttype skal give sin egen korte beskrivelse
        public abstract string GetSummary();
    }
}



