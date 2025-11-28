using System;

namespace Application.Features.Posts.Dtos
{
    public record PostDto(
        Guid Id,
        Guid AuthorId,
        string Title,
        string Body,
        string Category,
        bool IsPaywalled,
        bool IsPublished,
        DateTime CreatedAt
    );
}

