﻿namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        public static IEndpointRouteBuilder UseBlogRoutes(
            this IEndpointRouteBuilder endpoints)
        {
            // Định nghĩa route template, route constraint cho các
            // endpoint kết hợp với các action trong các controller.

            endpoints.MapControllerRoute(
                name: "newsletter-by-email",
                pattern: "newsletter/subscribe/{email}",
                defaults: new { controller = "Newsletter", action = "Subscribe" });

			endpoints.MapControllerRoute(
				name: "archives",
				pattern: "blog/archives/{year}/{month}",
				defaults: new { controller = "Blog", action = "Archives" });

			endpoints.MapControllerRoute(
                name: "posts-by-category",
                pattern: "blog/category/{slug}",
                defaults: new { controller = "Blog", action = "Category" });

            endpoints.MapControllerRoute(
                name: "posts-by-author",
                pattern: "blog/author/{slug}",
                defaults: new { controller = "Blog", action = "Author" });

            endpoints.MapControllerRoute(
                name: "posts-by-tag",
                pattern: "blog/tag/{slug}",
                defaults: new { controller = "Blog", action = "Tag" });

            endpoints.MapControllerRoute(
                name: "single-post",
                pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
                defaults: new { controller = "Blog", action = "Post" });

            endpoints.MapControllerRoute(
				name: "admin-area",
				pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
				defaults: new { area = "Admin" });

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            return endpoints;
        }
    }
}
