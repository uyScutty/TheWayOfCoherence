using Application.Features.Posts.Contracts;
using Application.Features.Posts.Commands;
using Domain.Content.Entities;
using MediatR;

namespace Application.Features.Posts.Commands
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _posts;

        public CreatePostCommandHandler(IPostRepository posts)
        {
            _posts = posts;
        }

        public async Task<Guid> Handle(CreatePostCommand cmd, CancellationToken ct)
        {
            var post = new Post(
                cmd.AuthorId,
                cmd.Title,
                cmd.Body,
                cmd.Category,
                cmd.IsPaywalled
            );

            if (cmd.PublishImmediately)
            {
                post.Publish();
            }

            var postId = await _posts.AddAsync(post);
            return postId;
        }
    }
}

