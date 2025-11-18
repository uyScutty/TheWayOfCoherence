using MediatR;

namespace Application.Features.Posts.Commands
{
    public sealed class PublishPostCommand : IRequest
    {
        public Guid PostId { get; }

        public PublishPostCommand(Guid postId)
        {
            PostId = postId;
        }
    }
}


