using Microsoft.EntityFrameworkCore;
using Delivery.API.Entities;

namespace Delivery.API.Data;

public class DeliveryDbContext : DbContext
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> o) : base(o) { }
    public DbSet<Entities.Delivery> Deliveries => Set<Entities.Delivery>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Entities.Delivery>(e =>
        {
            e.HasKey(d => d.Id);
            e.HasIndex(d => d.OrderId).IsUnique();
            e.Property(d => d.Status).HasConversion<int>();
        });

        var t = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        var driverId = Guid.Parse("a0000000-0000-0000-0000-000000000005");

        b.Entity<Entities.Delivery>().HasData(
            new Entities.Delivery
            {
                Id=Guid.Parse("d0000000-0000-0000-0000-000000000001"),
                OrderId=Guid.Parse("e0000000-0000-0000-0000-000000000001"),
                DriverId=driverId, DriverName="Arjun Singh", DriverPhone="9000000005",
                PickupAddress="78 Indiranagar, Bengaluru", DropAddress="12 MG Road, Bengaluru",
                CustomerEmail="rahul@food.com", Status=DeliveryStatus.Delivered,
                EstMins=20, AssignedAt=t.AddMinutes(38), PickedUpAt=t.AddMinutes(40),
                DeliveredAt=t.AddMinutes(50), UpdatedAt=t.AddMinutes(50)
            },
            new Entities.Delivery
            {
                Id=Guid.Parse("d0000000-0000-0000-0000-000000000002"),
                OrderId=Guid.Parse("e0000000-0000-0000-0000-000000000002"),
                DriverId=driverId, DriverName="Arjun Singh", DriverPhone="9000000005",
                PickupAddress="45 Koramangala, Bengaluru", DropAddress="45 Indiranagar, Bengaluru",
                CustomerEmail="priya@food.com", Status=DeliveryStatus.InTransit,
                EstMins=15, AssignedAt=t.AddHours(1).AddMinutes(25), PickedUpAt=t.AddHours(1).AddMinutes(28),
                UpdatedAt=t.AddHours(1).AddMinutes(28)
            }
        );
    }
}
