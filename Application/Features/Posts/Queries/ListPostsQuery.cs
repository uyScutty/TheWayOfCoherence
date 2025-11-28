using Application.Features.Posts.Dtos;
using MediatR;

namespace Application.Features.Posts.Queries
{
    public sealed class ListPostsQuery : IRequest<List<PostDto>>
    {
        public bool? IsPublished { get; }
        public string? Category { get; }

        public ListPostsQuery(bool? isPublished = null, string? category = null)
        {
            IsPublished = isPublished;
            Category = category;
        }
    }
}

