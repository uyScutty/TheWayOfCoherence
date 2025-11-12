using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content.Entities;

namespace Domain.Content.Factories
{
    public static class ContentFactory
    {
        // Minimum-API: tre overloads så Application kan skabe det rigtige uden new
        public static BlogPost CreateBlog(string title, Guid authorId, string body, string? imageUrl = null)
            => new BlogPost(authorId,title, body, imageUrl);

        public static VideoPost CreateVideo(string title, Guid authorId, string videoUrl, string? description = null)
            => new VideoPost(title, authorId, videoUrl, description);

        public static ReviewPost CreateReview(string title, string authorId, string productName, int rating, string reviewText)
            => new ReviewPost(title, authorId, productName, rating, reviewText);
    }
}

