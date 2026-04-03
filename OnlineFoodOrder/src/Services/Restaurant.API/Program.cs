using Restaurant.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApplicationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddSwagger()
    .AddApiServices();

var app = builder.Build();

// Configure pipeline
app.UseAppMiddleware();

await app.RunAsync();