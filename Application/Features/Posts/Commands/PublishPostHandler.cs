using Application.Features.Posts.Contracts;
using MediatR;

namespace Application.Features.Posts.Commands
{
    public class PublishPostCommandHandler : IRequestHandler<PublishPostCommand>
    {
        private readonly IPostRepository _posts;

        public PublishPostCommandHandler(IPostRepository posts)
        {
            _posts = posts;
        }

        public async Task Handle(PublishPostCommand cmd, CancellationToken ct)
        {
            var post = await _posts.GetByIdAsync(cmd.PostId);
            if (post == null)
                throw new InvalidOperationException("Post not found.");

            post.Publish();

            await _posts.UpdateAsync(post);
        }
    }
}


