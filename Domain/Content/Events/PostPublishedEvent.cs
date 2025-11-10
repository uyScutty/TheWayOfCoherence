using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Content.Events
{
    public class PostPublishedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public string Title { get; }

        public PostPublishedEvent(Guid postId, string title)
        {
            PostId = postId;
            Title = title;
        }
    }

    // Simpelt markeringsinterface til events
    public interface IDomainEvent { }

}
