using System.Text;
using Gateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer          = true, ValidIssuer   = builder.Configuration["Jwt:Issuer"],
        ValidateAudience        = true, ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime        = true
    });

builder.Services.AddCors(o =>
    o.AddPolicy("AllowWeb",
        p => p.WithOrigins("https://localhost:5010", "http://localhost:5010")
              .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddOcelot();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowWeb");
app.UseAuthentication();
app.MapGet("/health", () => new { service = "API Gateway", status = "healthy" });
await app.UseOcelot();

await app.RunAsync();
