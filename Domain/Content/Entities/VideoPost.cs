using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Content.Entities
{
    public class VideoPost : BasePost
    {
        public string VideoUrl { get; private set; }
        public string? Description { get; private set; }

        public VideoPost(string title, Guid authorId, string body,string videoUrl, string? description = null)
            : base(authorId, title, body)
        {
            VideoUrl = videoUrl;
            Description = description;
        }

        public override string GetSummary() => $"🎥 {Title}";
    }
}

