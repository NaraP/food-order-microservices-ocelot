using Delivery.API.Data;

namespace Delivery.API.Extensions
{
    public static class AppExtensions
    {
        public static void UseAppMiddleware(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
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

            app.MapGet("/health", () => new { service = "Delivery API", status = "healthy" });
        }
    }
}
