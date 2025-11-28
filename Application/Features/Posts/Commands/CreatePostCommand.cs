using MediatR;

namespace Application.Features.Posts.Commands
{
    public sealed class CreatePostCommand : IRequest<Guid>
    {
        public Guid AuthorId { get; }
        public string Title { get; }
        public string Body { get; }
        public string Category { get; }
        public bool IsPaywalled { get; }
        public bool PublishImmediately { get; }

        public CreatePostCommand(
            Guid authorId,
            string title,
            string body,
            string category,
            bool isPaywalled = false,
            bool publishImmediately = false)
        {
            AuthorId = authorId;
            Title = title;
            Body = body;
            Category = category;
            IsPaywalled = isPaywalled;
            PublishImmediately = publishImmediately;
        }
    }
}

