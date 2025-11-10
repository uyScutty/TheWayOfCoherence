using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Content.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<BasePost>
    {
        public void Configure(EntityTypeBuilder<BasePost> builder)
        {
            builder.HasDiscriminator<string>("PostType")
                .HasValue<BlogPost>("Blog")
                .HasValue<VideoPost>("Video")
                .HasValue<ReviewPost>("Review");
        }
    }
}

