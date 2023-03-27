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

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.Gender);

            builder.Property(c => c.Approved)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(c => c.PostedDate)
                .HasColumnType("datetime");

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .IsRequired();

			builder.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(p => p.PostId)
				.HasConstraintName("FK_Comments_Posts")
				.OnDelete(DeleteBehavior.Cascade);
		}
    }
}
