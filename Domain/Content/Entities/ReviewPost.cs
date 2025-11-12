using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Content.Entities
{
    public class ReviewPost : BasePost
    {
        public string ProductName { get; private set; }
        public int Rating { get; private set; } // 1–5
        public string ReviewText { get; private set; }
        private ReviewPost() { }

        public ReviewPost(Guid authorId, string title, string body, string productName, int rating, string reviewText)
            : base(authorId, title, body)
        {
            ProductName = productName;
            Rating = rating;
            ReviewText = reviewText;
        }

        public ReviewPost(string title, string authorId, string productName, int rating, string reviewText)
        {
            Title = title;
            AuthorId = authorId;
            ProductName = productName;
            Rating = rating;
            ReviewText = reviewText;
        }

        public override string GetSummary() => $"{ProductName} rated {Rating}/5";
    }

}
