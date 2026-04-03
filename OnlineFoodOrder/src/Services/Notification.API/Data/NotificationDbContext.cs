using Microsoft.EntityFrameworkCore;
using Notification.API.Entities;

namespace Notification.API.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> o) : base(o) { }
    public DbSet<NotificationLog> Logs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<NotificationLog>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(n => n.Email).HasMaxLength(256);
            e.Property(n => n.EventType).HasMaxLength(100);
        });
    }
}
