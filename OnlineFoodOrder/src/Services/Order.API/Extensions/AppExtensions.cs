using Order.API.Data;

namespace Order.API.Extensions;

public static class AppExtensions
{
    public static void UseAppMiddleware(this WebApplication app)
    {
        // Ensure DB created
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            db.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/health", () =>
            new { service = "Order API", status = "healthy" });
    }
}