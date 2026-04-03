using Microsoft.EntityFrameworkCore;
using Order.API.Entities;

namespace Order.API.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> o) : base(o) { }
    public DbSet<Entities.Order>       Orders  => Set<Entities.Order>();
    public DbSet<OrderItem>            Items   => Set<OrderItem>();
    public DbSet<OrderStatusHistory>   History => Set<OrderStatusHistory>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Entities.Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.HasIndex(o => o.OrderNumber).IsUnique();
            e.Property(o => o.SubTotal).HasColumnType("decimal(10,2)");
            e.Property(o => o.DeliveryFee).HasColumnType("decimal(10,2)");
            e.Property(o => o.Tax).HasColumnType("decimal(10,2)");
            e.Property(o => o.TotalAmount).HasColumnType("decimal(10,2)");
            e.Property(o => o.Status).HasConversion<int>();
            e.Property(o => o.PaymentStatus).HasConversion<int>();
        });
        b.Entity<OrderItem>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.UnitPrice).HasColumnType("decimal(10,2)");
            e.Property(i => i.TotalPrice).HasColumnType("decimal(10,2)");
            e.HasOne(i => i.Order).WithMany(o => o.Items)
             .HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
        });
        b.Entity<OrderStatusHistory>(e =>
        {
            e.HasKey(h => h.Id);
            e.Property(h => h.Status).HasConversion<int>();
            e.HasOne(h => h.Order).WithMany(o => o.History)
             .HasForeignKey(h => h.OrderId).OnDelete(DeleteBehavior.Cascade);
        });

        // ── Seed ──────────────────────────────────────────────────────────────
        var t = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        var cust1  = Guid.Parse("a0000000-0000-0000-0000-000000000002");
        var cust2  = Guid.Parse("a0000000-0000-0000-0000-000000000003");
        var rest1  = Guid.Parse("b1000000-0000-0000-0000-000000000003");
        var rest2  = Guid.Parse("b1000000-0000-0000-0000-000000000002");
        var item11 = Guid.Parse("d1000000-0000-0000-0000-000000000011");
        var item14 = Guid.Parse("d1000000-0000-0000-0000-000000000014");
        var item7  = Guid.Parse("d1000000-0000-0000-0000-000000000007");
        var item10 = Guid.Parse("d1000000-0000-0000-0000-000000000010");
        var ord1   = Guid.Parse("e0000000-0000-0000-0000-000000000001");
        var ord2   = Guid.Parse("e0000000-0000-0000-0000-000000000002");

        b.Entity<Entities.Order>().HasData(
            new Entities.Order { Id=ord1, OrderNumber="ORD-20240615-0001",
                CustomerId=cust1, CustomerName="Rahul Sharma", CustomerEmail="rahul@food.com", CustomerPhone="9000000002",
                RestaurantId=rest1, RestaurantName="Biryani Palace", DeliveryAddress="12 MG Road, Bengaluru",
                Status=OrderStatus.Delivered, PaymentStatus=PaymentStatus.Paid, PaymentMethod="UPI",
                SubTotal=600, DeliveryFee=35, Tax=30, TotalAmount=665, EstimatedMins=45,
                CreatedAt=t, ConfirmedAt=t.AddMinutes(3), DeliveredAt=t.AddMinutes(50), UpdatedAt=t.AddMinutes(50) },
            new Entities.Order { Id=ord2, OrderNumber="ORD-20240615-0002",
                CustomerId=cust2, CustomerName="Priya Nair", CustomerEmail="priya@food.com", CustomerPhone="9000000003",
                RestaurantId=rest2, RestaurantName="Pizza Hub", DeliveryAddress="45 Indiranagar, Bengaluru",
                Status=OrderStatus.OutForDelivery, PaymentStatus=PaymentStatus.Paid, PaymentMethod="Card",
                SubTotal=790, DeliveryFee=30, Tax=39.50m, TotalAmount=859.50m, EstimatedMins=40,
                CreatedAt=t.AddHours(1), ConfirmedAt=t.AddHours(1).AddMinutes(5), UpdatedAt=t.AddHours(1).AddMinutes(30) }
        );

        b.Entity<OrderItem>().HasData(
            new OrderItem { Id=Guid.NewGuid(), OrderId=ord1, MenuItemId=item11, Name="Hyderabadi Dum Biryani", Quantity=1, UnitPrice=320, TotalPrice=320 },
            new OrderItem { Id=Guid.NewGuid(), OrderId=ord1, MenuItemId=item14, Name="Butter Chicken",         Quantity=1, UnitPrice=280, TotalPrice=280 },
            new OrderItem { Id=Guid.NewGuid(), OrderId=ord2, MenuItemId=item7,  Name="BBQ Chicken Pizza",      Quantity=1, UnitPrice=450, TotalPrice=450 },
            new OrderItem { Id=Guid.NewGuid(), OrderId=ord2, MenuItemId=item10, Name="Chicken Alfredo",        Quantity=1, UnitPrice=340, TotalPrice=340 }
        );

        b.Entity<OrderStatusHistory>().HasData(
            new OrderStatusHistory { Id=1,  OrderId=ord1, Status=OrderStatus.Pending,        Note="Order placed",           Timestamp=t },
            new OrderStatusHistory { Id=2,  OrderId=ord1, Status=OrderStatus.Confirmed,      Note="Restaurant confirmed",   Timestamp=t.AddMinutes(3) },
            new OrderStatusHistory { Id=3,  OrderId=ord1, Status=OrderStatus.Preparing,      Note="Preparing your food",   Timestamp=t.AddMinutes(8) },
            new OrderStatusHistory { Id=4,  OrderId=ord1, Status=OrderStatus.OutForDelivery, Note="Out for delivery",       Timestamp=t.AddMinutes(38) },
            new OrderStatusHistory { Id=5,  OrderId=ord1, Status=OrderStatus.Delivered,      Note="Delivered",              Timestamp=t.AddMinutes(50) },
            new OrderStatusHistory { Id=6,  OrderId=ord2, Status=OrderStatus.Pending,        Note="Order placed",           Timestamp=t.AddHours(1) },
            new OrderStatusHistory { Id=7,  OrderId=ord2, Status=OrderStatus.Confirmed,      Note="Restaurant confirmed",   Timestamp=t.AddHours(1).AddMinutes(5) },
            new OrderStatusHistory { Id=8,  OrderId=ord2, Status=OrderStatus.OutForDelivery, Note="Driver on the way",      Timestamp=t.AddHours(1).AddMinutes(30) }
        );
    }
}
