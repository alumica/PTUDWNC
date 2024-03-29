﻿using System;
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

            //if (_dbContext.Posts.Any()) return;

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
                },
                new()
                {
                    FullName = "Bui Van Du",
                    UrlSlug = "bui-van-du",
                    Email = "2011368@dlu.edu.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Dang Ngoc Thang",
                    UrlSlug = "dang-ngoc-thang",
                    Email = "2012378@dlu.edu.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Huynh Tan Thanh",
                    UrlSlug = "huynh-tan-thanh",
                    Email = "2011439@dlu.edu.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Doan Cao Nhat Ha",
                    UrlSlug = "doan-cao-nhat-ha",
                    Email = "2012353@dlu.edu.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Nguyen Tuan Kiet",
                    UrlSlug = "nguyen-tuan-kiet",
                    Email = "2011400@dlu.edu.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Rick Anderson",
                    UrlSlug = "rick-anderson",
                    Email = "rickanderson99@motip.com",
                    JoinedDate = new DateTime(2020, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                },
                new()
                {
                    FullName = "Paul Andrew",
                    UrlSlug = "paul-andrew",
                    Email = "paulandrew99@motip.com",
                    JoinedDate = new DateTime(2021, 04, 19),
                    ImageUrl = "",
                    Notes = ""
                }
            };

            foreach (var author in authors)
            {
                if (!_dbContext.Authors.Any(a => a.UrlSlug == author.UrlSlug))
                    _dbContext.Authors.Add(author);
            }

            //_dbContext.Authors.AddRange(authors);
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
                new() { Name ="JAVA", Description="JAVA", UrlSlug="java", ShowOnMenu=true},
                new() { Name ="Design", Description="Design", UrlSlug="design", ShowOnMenu=true},
                new() { Name ="Technology", Description="Technology", UrlSlug="technology", ShowOnMenu=true},
                new() { Name ="AI", Description="Artificial Intelligence", UrlSlug="artificial-intelligence", ShowOnMenu=true},
            };

            foreach (var category in categories)
            {
                if (!_dbContext.Categories.Any(c => c.UrlSlug == category.UrlSlug))
                    _dbContext.Categories.Add(category);
            }

            // _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }

        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() {Name ="Google", Description="Google", UrlSlug ="google-application"},
                new() {Name ="ASP.NET MVC", Description="ASP.NET MVC", UrlSlug ="aspdotnet-mvc"},
                new() {Name ="Razor Page", Description="Razor Page", UrlSlug ="razor-page"},
                new() {Name ="Deep Learning", Description="Deep Learning", UrlSlug ="deep-learning"},
                new() {Name ="Neural Network", Description="Neural Network", UrlSlug ="neural-network"},
                new() {Name =".NET", Description=".NET", UrlSlug ="dotnet"},
                new() {Name ="Web apps", Description="Web apps", UrlSlug ="web-apps"},
                new() {Name ="Desktop apps", Description="Desktop apps", UrlSlug ="desktop-apps"},
                new() {Name ="Front End", Description="Front End", UrlSlug ="front-end"},
                new() {Name ="Back End", Description="Back End", UrlSlug ="back-end"},
                new() {Name ="Fullstack", Description="Fullstack", UrlSlug ="fullstack"},
            };


            foreach (var tag in tags)
            {
                if (!_dbContext.Tags.Any(t => t.UrlSlug == tag.UrlSlug))
                    _dbContext.Tags.Add(tag);
            }

            // _dbContext.AddRange(tags);
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
                    Title = "Part 3, add a view to an ASP.NET Core MVC app",
                    ShortDescription = "Add a view",
                    Description = "In this section, you modify the HelloWorldController class to use Razor view files. This cleanly encapsulates the process of generating HTML responses to a client.",
                    Meta = "",
                    UrlSlug = "adding-view",
                    ImageUrl = "images/post__adding-view.png",
                    Published = true,
                    PostedDate = new DateTime(2023, 03, 03, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[9],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[5],
                        tags[6]
                    }
                },
                new()
                {
                    Title = "Part 2, add a controller to an ASP.NET Core MVC app",
                    ShortDescription = "Add a controller",
                    Description = "The Model-View-Controller (MVC) architectural pattern separates an app into three main components: Model, View, and Controller. The MVC pattern helps you create apps that are more testable and easier to update than traditional monolithic apps.",
                    Meta = "",
                    UrlSlug = "adding-controller",
                    ImageUrl = "",
                    Published = true,
                    PostedDate = new DateTime(2023, 03, 03, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[9],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[5],
                        tags[6]
                    }
                },
                new()
                {
                    Title = "Get started with ASP.NET Core MVC",
                    ShortDescription = "Get started",
                    Description = "This tutorial teaches ASP.NET Core MVC web development with controllers and views. If you're new to ASP.NET Core web development, consider the Razor Pages version of this tutorial, which provides an easier starting point. See Choose an ASP.NET Core UI, which compares Razor Pages, MVC, and Blazor for UI development.",
                    Meta = "",
                    UrlSlug = "start-mvc",
                    ImageUrl = "",
                    Published = true,
                    PostedDate = new DateTime(2023, 03, 03, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[9],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[5],
                        tags[6]
                    }
                },
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

            foreach (var post in posts)
            {
                if (!_dbContext.Posts.Any(p => p.UrlSlug == post.UrlSlug))
                    _dbContext.Posts.Add(post);
            }

            // _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }


    }
}
