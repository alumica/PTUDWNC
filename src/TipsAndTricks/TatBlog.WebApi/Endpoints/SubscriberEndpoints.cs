using FluentEmail.Core;
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
    public static class SubscriberEndpoints
    {
        public static WebApplication MapSubscriberEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/subscribers");

            routeGroupBuilder.MapGet("/", GetSubscribers)
                .WithName("GetSubscribers")
                .Produces<PaginationResult<Subscriber>>();

            routeGroupBuilder.MapGet("/{id:int}", GetSubscriberDetails)
                .WithName("GetSubscriberById")
                .Produces<Subscriber>()
                .Produces(404);

            routeGroupBuilder.MapPost("/subscribe/{email}", Subscribe)
               .WithName("AddNewSubscriber")
               .Produces(201)
               .Produces(400)
               .Produces(409);

            routeGroupBuilder.MapPost("/unsubscribe/{email}", Unsubscribe)
               .WithName("AddNewUnSubscriber")
               .Produces(201)
               .Produces(400)
               .Produces(409);

            routeGroupBuilder.MapPost("/blocksubscribe/{email}", BlockSubscribe)
               .WithName("BlockSubscriber")
               .Produces(201)
               .Produces(400)
               .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteSubscriber)
              .WithName("DeleteSubscriber")
              .Produces(204)
              .Produces(404);

            return app;
        }

        private static async Task<IResult> GetSubscribers(
            [AsParameters] SubscriberFilterModel model,
            ISubscriberRepository subscriberRepository)
        {
            var subscribersList = await subscriberRepository
                .GetPagedSubscribersAsync(model);

            var paginationResult =
                new PaginationResult<Subscriber>(subscribersList);
            
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> Subscribe(
            string email,
            ISubscriberRepository subscriberRepository,
            IMapper mapper)
        {
            if (await subscriberRepository
                .IsExistedEmail(email))
            {
                return Results.Conflict(
                    $"Email '{email}' đã được sử dụng");
            }

            Subscriber subscriber = new Subscriber()
            {
                Email = email,
                SubscribeDate = DateTime.Now,
            };
            
            await subscriberRepository.SubscribeAsync(subscriber);

            return Results.CreatedAtRoute(
                "GetSubscriberById", new { subscriber.Id },
                mapper.Map<Subscriber>(subscriber));
        }

        private static async Task<IResult> Unsubscribe(
            string email,
            ISubscriberRepository subscriberRepository,
            IMapper mapper)
        {
            if (!await subscriberRepository
                .IsExistedEmail(email))
            {
                return Results.Conflict(
                    $"Email '{email}' không tồn tại trên hệ thống");
            }

            return await subscriberRepository.UnsubscribeAsync(email)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy người đăng ký với email = {email}");
        }

        private static async Task<IResult> BlockSubscribe(
            string email,
            SubscriberEditModel model,
            ISubscriberRepository subscriberRepository,
            IMapper mapper)
        {
            if (!await subscriberRepository
                .IsExistedEmail(email))
            {
                return Results.Conflict(
                    $"Email '{email}' không tồn tại trên hệ thống");
            }

            var subscriber = mapper.Map<Subscriber>(model);
            subscriber.Email = email;

            return await subscriberRepository.UnsubscribeAsync(email, subscriber.ResonUnsubscribe, false)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy người đăng ký với email = {email}");
        }

        private static async Task<IResult> GetSubscriberDetails(
            int id,
            ISubscriberRepository subscriberRepository,
            IMapper mapper)
        {
            var category = await subscriberRepository.GetCachedSubscriberByIdAsync(id);

            return category == null
                ? Results.NotFound($"Không tìm thấy người đăng ký có mã số {id}")
                : Results.Ok(mapper.Map<Subscriber>(category));
        }

        private static async Task<IResult> DeleteSubscriber(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCategoryByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy tác giả với mã = {id}");
        }
    }
}