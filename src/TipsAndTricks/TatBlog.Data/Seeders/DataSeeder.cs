using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug = "json-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Olia Gavrysh",
                    UrlSlug = "olia-gavrysh",
                    Email = "olia665@motip.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Steve Smith",
                    UrlSlug = "steve-smith",
                    Email = "steves554@motip.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                }
            };

            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }

        private IList<Category> AddCategories() 
        {
            var categories = new List<Category>()
            {
                new() { Name =".NET Core", Description=".NET Core", UrlSlug="dotnet", ShowOnMenu=true},
                new() { Name ="ASP.NET", Description=".NET Core", UrlSlug="aspnet", ShowOnMenu=true},
                new() { Name ="Architecture", Description="Architecture", UrlSlug="architecture", ShowOnMenu=true},
                new() { Name ="Messaging", Description="Messaging", UrlSlug="messaging", ShowOnMenu=true},
                new() { Name ="OOP", Description="Object-Oriented Program", UrlSlug="oop", ShowOnMenu=true},
            };

            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }

        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() {Name ="Google", Description="Google Application", UrlSlug ="google-application"},
                new() {Name ="ASP.NET MVC", Description="ASP.NET MVC", UrlSlug ="aspdotnet-mvc"},
                new() {Name ="Razor Page", Description="Razor Page", UrlSlug ="razor-page"},
                new() {Name ="Deep Learning", Description="Deep Learning", UrlSlug ="deep-learning"},
                new() {Name ="Neural Network", Description="Neural Network", UrlSlug ="neural-network"}
            };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();
            return tags;
        }

        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "Layout in ASP.NET Core",
                    ShortDescription = "This document discusses layouts for the two different approaches to ASP.NET Core MVC: Razor Pages and controllers with views.",
                    Description = "What is a Layout\nMost web apps have a common layout that provides the user with a consistent experience as they navigate from page to page. The layout typically includes common user interface elements such as the app header, navigation or menu elements, and footer.",
                    Meta = "",
                    UrlSlug = "layout-in-aspnet-core",
                    ImageUrl = "",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[3],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },
                new()
                {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great ...",
                    Description = "Here's a few great DON'T and DO examples ...",
                    Meta = "David and friends has a great respository filled ...",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    ImageUrl = "",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                new()
                {
                    Title = "Upgrading your .NET projects with Visual Studio",
                    ShortDescription = "Upgrading your .NET projects with Visual Studio",
                    Description = "Now you can upgrade any .NET application to the latest version of .NET inside of Visual Studio! We are happy to introduce it as a Visual Studio extension and will upgrade your .NET Framework or .NET Core web- and desktop apps. Some project types are in development and coming soon, see the details below.",
                    Meta = "Upgrading your .NET projects with Visual Studio",
                    UrlSlug = "upgrade-assistant-now-in-visual-studio",
                    ImageUrl = "",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 15, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[2],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                }
            };
            
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }


    }
}
