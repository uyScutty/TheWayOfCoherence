using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Posts.Contracts;
namespace Application.Features.Posts.Commands
{
    /// <summary>
    /// Use-case for at udgive et Post.
    /// Rejser et domain event (PostPublishedEvent), som håndteres i Application-laget.
    /// </summary>
    public class PublishPostHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public PublishPostHandler(IPostRepository postRepository, IDomainEventDispatcher eventDispatcher)
        {
            _postRepository = postRepository;
            _eventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// Kaldes fra UI (fx Publish-knap) for at publicere et Post.
        /// </summary>
        public async Task HandleAsync(Guid postId)
        {
            // 1️⃣ Hent posten fra databasen
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found.");

            // 2️⃣ Udgiv posten (rejser et Domain Event internt)
            post.Publish();

            // 3️⃣ Gem ændringen i databasen
            await _postRepository.UpdateAsync(post);

            // 4️⃣ Dispatch alle rejste Domain Events
            foreach (var domainEvent in post.DomainEvents)
                await _eventDispatcher.DispatchAsync(domainEvent);

            // 5️⃣ Ryd listen, så events ikke rejses igen ved næste save
            post.ClearDomainEvents();
        }
    }
}

