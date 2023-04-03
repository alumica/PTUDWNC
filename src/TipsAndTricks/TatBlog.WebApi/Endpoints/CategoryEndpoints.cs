using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapGet(
                    "/{slug:regex(^[a-z0-9_-]*$)}/posts",
                    GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlugs")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddNewCategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                .WithName("UpdateAnCategory")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                .WithName("DeleteAnCategory")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetCategories(
            [AsParameters] CategoryFilterModel model,
            IBlogRepository blogRepository)
        {
            if (model.IsPaged)
            {
                var categoriesList = await blogRepository
                .GetPagedCategoriesAsync(model, model.Name);

                var paginationResult =
                    new PaginationResult<CategoryItem>(categoriesList);

                return Results.Ok(ApiResponse.Success(paginationResult));
            }
            else
            {
                var categoriesList = await blogRepository
                .GetCategoriesAsync();

                return Results.Ok(ApiResponse.Success(categoriesList));
            }
        }

        private static async Task<IResult> GetCategoryDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var category = await blogRepository.GetCachedCategoryByIdAsync(id);

            return category == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm chuyên mục có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(category)));
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

            return Results.Ok(ApiResponse.Success(paginationResult));
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

            return Results.Ok(ApiResponse.Success(
                mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }

     

        private static async Task<IResult> UpdateCategory(
            int id,
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IValidator<CategoryEditModel> validator,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
            }

            if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await blogRepository.AddOrUpdateCategoryAsync(category)
                ? Results.Ok(ApiResponse.Success("Chuyên mục được cập nhật", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy chuyên mục"));
        }

        private static async Task<IResult> DeleteCategory(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCategoryByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Chuyên mục đã được xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy chuyên mục"));
        }
    }
}