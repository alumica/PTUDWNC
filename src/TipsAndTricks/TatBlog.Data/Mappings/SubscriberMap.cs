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
    public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToTable("Subscribers");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.SubscribeDate)
                .HasColumnType("datetime");

            builder.Property(a => a.UnsubscribeDate)
                .HasColumnType("datetime");

            builder.Property(a => a.ResonUnsubscribe)
                .HasMaxLength(500);

            builder.Property(a => a.TypeReason);

            builder.Property(a => a.Notes)
                .HasMaxLength(500);
        }
    }
}
