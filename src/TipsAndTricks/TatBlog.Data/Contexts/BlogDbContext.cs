﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Mappings;

namespace TatBlog.Data.Contexts
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public BlogDbContext() { }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            // Bạn phải thay đổi chuổi kết nối cho phù hợp
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-OF-KIET\SQLEXPRESS;Initial Catalog=TatBlog;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CategoryMap).Assembly);
        }
    }
}
