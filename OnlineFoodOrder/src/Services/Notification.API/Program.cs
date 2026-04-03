using Microsoft.EntityFrameworkCore;
using Notification.API.Consumers;
using Notification.API.Data;
using FoodOrder.Shared.Contracts;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<NotificationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
using (var scope = host.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<NotificationDbContext>().Database.EnsureCreated();
await host.RunAsync();
