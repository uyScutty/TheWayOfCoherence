using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;

namespace Domain.Content.Events
{
    public class PostPublishedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public string Title { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public PostPublishedEvent(Guid postId, string title)
        {
            PostId = postId;
            Title = title;
        }
    }
}
