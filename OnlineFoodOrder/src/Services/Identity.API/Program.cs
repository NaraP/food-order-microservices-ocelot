using Identity.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApplicationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerWithAuth()
    .AddApiServices();

var app = builder.Build();

// Configure pipeline
app.UseAppMiddleware();

await app.RunAsync();