using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder
        .ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValition();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.SetupRequestPipeline();

    // Configure API endpoints
    app.MapCategoryEndpoints();
    app.MapAuthorEndpoints();
    app.MapPostEndpoints();
    app.MapTagEndpoints();
    app.MapSubscriberEndpoints();
    app.MapCommentEndpoints();
    app.MapStatisticsEndpoints();

    app.Run();
}