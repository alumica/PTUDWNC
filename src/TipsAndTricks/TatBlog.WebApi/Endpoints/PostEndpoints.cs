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
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
                .WithName("GetPosts")
                .Produces<ApiResponse<PaginationResult<Post>>>();

            routeGroupBuilder.MapGet("/featured/{limit}", GetFeaturedPosts)
                .WithName("GetPopularPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<Post>>>();

            routeGroupBuilder.MapGet("/archives/{limit}", GetArchivesPosts)
                .WithName("GetArchivesPosts")
                .Produces<ApiResponse<IList<PostItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                .WithName("GetPostById")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostDetailsBySlug)
                .WithName("GetPosttBySlug")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetPostPicture)
                .WithName("SetPostPicture")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                .WithName("UpdateAnPost")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeleteAnPost")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/{id}/comments", GetCommentsPost)
                .WithName("GetCommentsPost")
                .Produces<ApiResponse<IList<Comment>>>();

            return app;
        }

        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var postQuery = mapper.Map<PostQuery>(model);


            var postsList = await blogRepository.GetPagedPostQueryAsync(
               postQuery, model,
               posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetFeaturedPosts(
            int limit,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var featured = await blogRepository.GetPopularArticlesAsync(limit);
            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(featured)));
        }

        private static async Task<IResult> GetRandomPosts(
            int limit,
            IBlogRepository blogRepository,
            IMapper mapper )
        {
            var radnom = await blogRepository.GetRandomNPostsAsync(limit);
            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(radnom)));
        }

        private static async Task<IResult> GetArchivesPosts(
            int limit,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var radnom = await blogRepository.CountPostsNMonthAsync(limit);
            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostItem>>(radnom)));
        }

        private static async Task<IResult> GetPostDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var post = await blogRepository.GetCachedPostByIdAsync(id);

            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));

        }

        private static async Task<IResult> GetPostDetailsBySlug(
            string slug,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var post = await blogRepository.GetCachedPostBySlugAsync(slug);

            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã định danh {slug}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
        }

        private static async Task<IResult> AddPost(
            PostEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được  sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            await blogRepository.AddOrUpdatePostAsync(post);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<PostDto>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetPostPicture(
            int id, 
            IFormFile imageFile,
            IBlogRepository blogRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl)) 
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdatePost(
            int id,
            PostEditModel model,
            IValidator<PostEditModel> validator,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
            }

            if (await blogRepository
                .IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            post.Id = id;

            return await blogRepository.AddOrUpdatePostAsync(post)
                ? Results.Ok(ApiResponse.Success("Bài viết được cập nhật", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy bài viết"));
        }

        private static async Task<IResult> DeletePost(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Bài viết được cập nhật", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy bài viết"));
        }

        private static async Task<IResult> GetCommentsPost(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var commentsList = await blogRepository.GetCommentsByPostIdAsync(id);

            return Results.Ok(ApiResponse.Success(commentsList));
        }
    }
}