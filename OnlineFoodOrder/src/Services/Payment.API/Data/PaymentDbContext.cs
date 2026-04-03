// ── DbContext ─────────────────────────────────────────────────────────────────
namespace Payment.API.Data;
using Microsoft.EntityFrameworkCore;
using Payment.API.Entities;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> o) : base(o) { }
    public DbSet<Entities.Payment> Payments => Set<Entities.Payment>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Entities.Payment>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.OrderId);
            e.Property(p => p.Amount).HasColumnType("decimal(10,2)");
            e.Property(p => p.Method).HasConversion<int>();
            e.Property(p => p.State).HasConversion<int>();
        });

        var t = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        b.Entity<Entities.Payment>().HasData(
            new Entities.Payment { Id=Guid.Parse("f0000000-0000-0000-0000-000000000001"),
                OrderId=Guid.Parse("e0000000-0000-0000-0000-000000000001"),
                CustomerId=Guid.Parse("a0000000-0000-0000-0000-000000000002"),
                CustomerEmail="rahul@food.com", Amount=665,
                Method=PaymentMethod.UPI, State=PaymentState.Completed,
                TransactionId="TXN-UPI-20240615-001", CreatedAt=t, CompletedAt=t.AddSeconds(30) },
            new Entities.Payment { Id=Guid.Parse("f0000000-0000-0000-0000-000000000002"),
                OrderId=Guid.Parse("e0000000-0000-0000-0000-000000000002"),
                CustomerId=Guid.Parse("a0000000-0000-0000-0000-000000000003"),
                CustomerEmail="priya@food.com", Amount=859.50m,
                Method=PaymentMethod.Card, State=PaymentState.Completed,
                TransactionId="TXN-CRD-20240615-002", CreatedAt=t.AddHours(1), CompletedAt=t.AddHours(1).AddSeconds(45) }
        );
    }
}
