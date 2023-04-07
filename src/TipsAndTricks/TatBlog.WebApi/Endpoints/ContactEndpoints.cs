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
using TatBlog.Services.Contacts;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class ContactEndpoints
    {
        public static WebApplication MapContactEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/contacts");

            routeGroupBuilder.MapGet("/", GetContacts)
                .WithName("GetContacts")
                .Produces<ApiResponse<PaginationResult<ContactItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetContactDetails)
                .WithName("GetContactById")
                .Produces<ApiResponse<ContactItem>>();

            routeGroupBuilder.MapPost("/", AddContact)
                .WithName("AddNewContact")
                .AddEndpointFilter<ValidatorFilter<ContactEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<ContactItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateContact)
                .WithName("UpdateAnContact")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteContact)
                .WithName("DeleteAnContact")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetContacts(
            IContactRepository contactRepository)
        {
            var contacts = await contactRepository.GetContactsAsync(); 
            return Results.Ok(ApiResponse.Success(contacts));
        }

        private static async Task<IResult> GetContactDetails(
            int id,
            IContactRepository contactRepository,
            IMapper mapper)
        {
            var contact = await contactRepository.GetCachedContactByIdAsync(id);

            return contact == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm liên hệ có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(contact)));
        }

        private static async Task<IResult> AddContact(
            ContactEditModel model,
            IContactRepository contactRepository,
            IMapper mapper)
        {

            var contact = mapper.Map<Contact>(model);
            contact.ContactDate = DateTime.Now;
            await contactRepository.AddOrUpdateContactAsync(contact);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<ContactItem>(contact), HttpStatusCode.Created));
        }

     

        private static async Task<IResult> UpdateContact(
            int id,
            ContactEditModel model,
            IContactRepository contactRepository,
            IValidator<ContactEditModel> validator,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
            }

            var contact = mapper.Map<Contact>(model);
            contact.Id = id;

            return await contactRepository.AddOrUpdateContactAsync(contact)
                ? Results.Ok(ApiResponse.Success("Người liên hệ đã được xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy liên hệ"));
        }

        private static async Task<IResult> DeleteContact(
            int id,
            IContactRepository contactRepository)
        {
            return await contactRepository.DeleteContactByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Người liên hệ đã được xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy liên hệ"));
        }
    }
}