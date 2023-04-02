using Azure;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
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
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
                .WithName("GetTags")
                .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
                .WithName("GetTagById")
                .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapGet(
                    "/{slug:regex(^[a-z0-9_-]*$)}/tags",
                    GetPostsByTagSlug)
                .WithName("GetPostsByTagSlugs")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddTag)
               .WithName("AddNewTag")
               .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
               .Produces(401)
               .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
              .WithName("UpdateAnTag")
              .Produces(401)
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
              .WithName("DeleteAnTag")
              .Produces(401)
              .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetTags(
            [AsParameters] TagFilterModel model,
            IBlogRepository blogRepository)
        {  
            var tagsList = await blogRepository
                .GetPagedTagsAsync(model, model.Name);

            var paginationResult =
                new PaginationResult<TagItem>(tagsList);
            
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetTagDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var tag = await blogRepository.GetCachedTagByIdAsync(id);

            return tag == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }

        private static async Task<IResult> GetPostsByTagSlug(
            string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                TagSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostQueryAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddTag(
            TagEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsTagSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được  sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            await blogRepository.AddOrUpdateTagAsync(tag);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<TagItem>(tag), HttpStatusCode.Created));
        }



        private static async Task<IResult> UpdateTag(
            int id,
            TagEditModel model,
            IBlogRepository blogRepository,
            IValidator<TagEditModel> validator,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
            }

            if (await blogRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await blogRepository.AddOrUpdateTagAsync(tag)
                ? Results.Ok(ApiResponse.Success("Thẻ được cập nhật", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy thẻ"));
        }

        private static async Task<IResult> DeleteTag(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteTagByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Thẻ đã được xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy thẻ"));
        }
    }
}