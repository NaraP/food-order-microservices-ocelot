using Delivery.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApplicationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

// Configure middleware
app.UseAppMiddleware();

await app.RunAsync();
