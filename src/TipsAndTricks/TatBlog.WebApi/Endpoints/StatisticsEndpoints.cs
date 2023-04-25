using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Subscribers;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class StatisticsEndpoints
    {
        public static WebApplication MapStatisticsEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/statistics");

            routeGroupBuilder.MapGet("/totalpost", GetTotalPost)
                .WithName("GetTotalPost")
                .Produces<ApiResponse<PaginationResult<int>>>();

            routeGroupBuilder.MapGet("/postunpublished", GetNumberPostsUnpublished)
                .WithName("GetNumberPostsUnpublished")
                .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/categories", GetNumberCategories)
                .WithName("GetNumberCategories")
                .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/authors", GetNumberAuthors)
                .WithName("GetNumberAuthors")
                .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/commentunapproved", GetNumberCommentsUnapproved)
                .WithName("GetNumberCommentsUnapproved")
                .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/subscribers", GetNumberSubscribers)
                .WithName("GetNumberSubscribers")
                .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/subscriberstoday", GetNumberSubscribersToday)
                .WithName("GetNumberSubscribersToday")
                .Produces<ApiResponse<int>>();
            return app;
        }

        private static async Task<IResult> GetTotalPost(
            IBlogRepository blogRepository)
        {
            int total = await blogRepository.GetTotalPostsAsync();

            return Results.Ok(ApiResponse.Success(total));
        }

        private static async Task<IResult> GetNumberPostsUnpublished(
            IBlogRepository blogRepository)
        {
            int number = await blogRepository.NumberPostsUnpublishedAsync();

            return Results.Ok(ApiResponse.Success(number));
        }

        private static async Task<IResult> GetNumberCategories(
            IBlogRepository blogRepository)
        {
            int number = await blogRepository.NumberCategoriesAsync();

            return Results.Ok(ApiResponse.Success(number));
        }

        private static async Task<IResult> GetNumberAuthors(
            IAuthorRepository authorRepository)
        {
            int number = await authorRepository.NumberAuthorsAsync();

            return Results.Ok(ApiResponse.Success(number));
        }


        private static async Task<IResult> GetNumberCommentsUnapproved(
            IBlogRepository blogRepository)
        {
            int number = await blogRepository.NumberCommentsUnApprovedAsync();

            return Results.Ok(ApiResponse.Success(number));
        }

        private static async Task<IResult> GetNumberSubscribers(
            ISubscriberRepository subscriberRepository)
        {
            int number = await subscriberRepository.NumberSubscribersAsync();

            return Results.Ok(ApiResponse.Success(number));
        }


        private static async Task<IResult> GetNumberSubscribersToday(
            ISubscriberRepository subscriberRepository)
        {
            int number = await subscriberRepository.NumberSubscribersTodayAsync();

            return Results.Ok(ApiResponse.Success(number));
        }
    }
}