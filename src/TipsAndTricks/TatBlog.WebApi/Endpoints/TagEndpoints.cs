using Azure;
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
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
                .WithName("GetTags")
                .Produces<PaginationResult<TagItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
                .WithName("GetTagById")
                .Produces<TagItem>()
                .Produces(404);

            routeGroupBuilder.MapGet(
                    "/{slug:regex(^[a-z0-9_-]*$)}/tags",
                    GetTagsByCategorySlug)
                .WithName("GetPostsByTagSlugs")
                .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddTag)
               .WithName("AddNewTag")
               .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
               .Produces(201)
               .Produces(400)
               .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
              .WithName("UpdateAnTag")
              .Produces(204)
              .Produces(400)
              .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
              .WithName("DeleteAnTag")
              .Produces(204)
              .Produces(404);

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
            
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetTagDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var tag = await blogRepository.GetCachedTagByIdAsync(id);

            return tag == null
                ? Results.NotFound($"Không tìm thấy thẻ/tag có mã số {id}")
                : Results.Ok(mapper.Map<TagItem>(tag));
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

        private static async Task<IResult> GetTagsByCategorySlug(
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

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddTag(
            TagEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsTagSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var tag = mapper.Map<Tag>(model);
            await blogRepository.AddOrUpdateTagAsync(tag);

            return Results.CreatedAtRoute(
                "GetTagById", new { tag.Id },
                mapper.Map<TagItem>(tag));
        }



        private static async Task<IResult> UpdateTag(
            int id,
            TagEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsTagSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await blogRepository.AddOrUpdateTagAsync(tag)
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteTag(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteTagByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy thẻ với mã = {id}");
        }
    }
}