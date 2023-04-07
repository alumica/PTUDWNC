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
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Subject)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(a => a.ContactDate)
                .HasColumnType("datetime");
        }
    }
}
