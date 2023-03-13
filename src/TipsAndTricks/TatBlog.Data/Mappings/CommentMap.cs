using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Gender);

            builder.Property(p => p.Approved)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(p => p.PostedDate)
                .HasColumnType("datetime");

            builder.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
