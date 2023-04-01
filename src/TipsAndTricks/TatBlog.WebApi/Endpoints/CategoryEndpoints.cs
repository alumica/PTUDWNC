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
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<PaginationResult<CategoryItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<CategoryItem>()
                .Produces(404);

            routeGroupBuilder.MapGet(
                    "/{slug:regex(^[a-z0-9_-]*$)}/posts",
                    GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlugs")
                .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddCategory)
               .WithName("AddNewCategory")
               .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
               .Produces(201)
               .Produces(400)
               .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
              .WithName("UpdateAnCategory")
              .Produces(204)
              .Produces(400)
              .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
              .WithName("DeleteAnCategory")
              .Produces(204)
              .Produces(404);

            return app;
        }

        private static async Task<IResult> GetCategories(
            [AsParameters] CategoryFilterModel model,
            IBlogRepository blogRepository)
        {
            var categoriesList = await blogRepository
                .GetPagedCategoriesAsync(model, model.Name);

            var paginationResult =
                new PaginationResult<CategoryItem>(categoriesList);
            
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetCategoryDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var category = await blogRepository.GetCachedCategoryByIdAsync(id);

            return category == null
                ? Results.NotFound($"Không tìm thấy chủ đề có mã số {id}")
                : Results.Ok(mapper.Map<CategoryItem>(category));
        }

        private static async Task<IResult> GetPostsByAuthorId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorId = id,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostQueryAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetPostsByCategorySlug(
            string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostQueryAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddCategory(
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            await blogRepository.AddOrUpdateCategoryAsync(category);

            return Results.CreatedAtRoute(
                "GetCategoryById", new { category.Id },
                mapper.Map<CategoryItem>(category));
        }

     

        private static async Task<IResult> UpdateCategory(
            int id,
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await blogRepository.AddOrUpdateCategoryAsync(category)
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteCategory(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCategoryByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy tác giả với mã = {id}");
        }
    }
}