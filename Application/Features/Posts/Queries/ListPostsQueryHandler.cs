using Application.Features.Posts.Contracts;
using Application.Features.Posts.Dtos;
using Application.Features.Posts.Queries;
using MediatR;

namespace Application.Features.Posts.Queries
{
    public class ListPostsQueryHandler : IRequestHandler<ListPostsQuery, List<PostDto>>
    {
        private readonly IPostRepository _posts;

        public ListPostsQueryHandler(IPostRepository posts)
        {
            _posts = posts;
        }

        public async Task<List<PostDto>> Handle(ListPostsQuery query, CancellationToken ct)
        {
            var posts = await _posts.ListAsync(query.IsPublished, query.Category);

            return posts.Select(p => new PostDto(
                p.Id,
                p.AuthorId,
                p.Title,
                p.Body,
                p.Category,
                p.IsPaywalled,
                p.IsPublished,
                p.CreatedAt
            )).ToList();
        }
    }
}

