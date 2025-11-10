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

        public VideoPost(string title, string authorId, string videoUrl, string? description = null)
            : base(title, authorId)
        {
            VideoUrl = videoUrl;
            Description = description;
        }

        public override string GetSummary() => $"🎥 {Title}";
    }
}

