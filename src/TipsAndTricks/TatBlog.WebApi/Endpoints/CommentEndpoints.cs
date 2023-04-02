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
    public static class CommentEndpoints
    {
        public static WebApplication MapCommentEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/comments");

            routeGroupBuilder.MapGet("/", GetComments)
                .WithName("GetComments")
                .Produces<ApiResponse<PaginationResult<Comment>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCommentDetails)
                .WithName("GetCommentById")
                .Produces<ApiResponse<Comment>>();

            routeGroupBuilder.MapPost("/", AddComment)
                .WithName("AddNewComment")
                .AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<Comment>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateComment)
                .WithName("UpdateAnComment")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
                .WithName("DeleteAnComment")
                .Produces(401)
                .Produces<ApiResponse<string>>();

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

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetCommentDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var comment = await blogRepository.GetCachedCommentByIdAsync(id);

            return comment == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bình luận có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<Comment>(comment)));
        }

        private static async Task<IResult> AddComment(
            CommentEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var comment = mapper.Map<Comment>(model);
            await blogRepository.AddOrUpdateCommentAsync(comment);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<Comment>(comment), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateComment(
            int id,
            CommentEditModel model,
            IBlogRepository blogRepository,
            IValidator<CommentEditModel> validator,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
            }

            var comment = mapper.Map<Comment>(model);
            comment.Id = id;

            return await blogRepository.AddOrUpdateCommentAsync(comment)
                 ? Results.Ok(ApiResponse.Success("Bình luận đã được cập nhật", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy bình luận"));
        }

        private static async Task<IResult> DeleteComment(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCommentByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Bình luận đã được xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy bình luận"));
        }
    }
}