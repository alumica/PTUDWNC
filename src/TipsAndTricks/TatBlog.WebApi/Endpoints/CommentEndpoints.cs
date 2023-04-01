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
    public static class CommentEndpoints
    {
        public static WebApplication MapCommentEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/comments");

            routeGroupBuilder.MapGet("/", GetComments)
                .WithName("GetComments")
                .Produces<PaginationResult<Comment>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCommentDetails)
                .WithName("GetCommentById")
                .Produces<Comment>()
                .Produces(404);

            //routeGroupBuilder.MapGet(
            //        "/{slug:regex(^[a-z0-9_-]*$)}/posts",
            //        GetPostsByCategorySlug)
            //    .WithName("GetPostsByCategorySlugs")
            //    .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddComment)
               .WithName("AddNewComment")
               .AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
               .Produces(201)
               .Produces(400)
               .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateComment)
              .WithName("UpdateAnComment")
              .Produces(204)
              .Produces(400)
              .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
              .WithName("DeleteAnComment")
              .Produces(204)
              .Produces(404);

            return app;
        }

        private static async Task<IResult> GetComments(
            [AsParameters] CommentFilterModel model,
            IBlogRepository blogRepository)
        {
            var commentsList = await blogRepository
                .GetPagedCommentAsync(model);

            var paginationResult =
                new PaginationResult<Comment>(commentsList);
            
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetCommentDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var comment = await blogRepository.GetCachedCommentByIdAsync(id);

            return comment == null
                ? Results.NotFound($"Không tìm thấy bình luận có mã số {id}")
                : Results.Ok(mapper.Map<Comment>(comment));
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

        private static async Task<IResult> AddComment(
            CommentEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var comment = mapper.Map<Comment>(model);
            await blogRepository.AddOrUpdateCommentAsync(comment);

            return Results.CreatedAtRoute(
                "GetCommentById", new { comment.Id },
                mapper.Map<Comment>(comment));
        }

     

        private static async Task<IResult> UpdateComment(
            int id,
            CommentEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var comment = mapper.Map<Comment>(model);
            comment.Id = id;

            return await blogRepository.AddOrUpdateCommentAsync(comment)
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteComment(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCommentByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy bình luận với mã = {id}");
        }
    }
}