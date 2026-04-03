using Microsoft.EntityFrameworkCore;
using Identity.API.Entities;

namespace Identity.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(256);
            e.Property(u => u.Role).HasMaxLength(50);
        });

        // Password for all seed users: Food@1234
        const string hash = "$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2uheWG/igi.";
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        b.Entity<User>().HasData(
            new User { Id = Guid.Parse("a0000000-0000-0000-0000-000000000001"), FirstName = "Admin",   LastName = "User",    Email = "admin@food.com",    PasswordHash = hash, PhoneNumber = "9000000001", Role = "Admin",           Address = "HQ, Bengaluru",              IsActive = true, CreatedAt = now },
            new User { Id = Guid.Parse("a0000000-0000-0000-0000-000000000002"), FirstName = "Rahul",   LastName = "Sharma",  Email = "rahul@food.com",    PasswordHash = hash, PhoneNumber = "9000000002", Role = "Customer",        Address = "12 MG Road, Bengaluru",      IsActive = true, CreatedAt = now },
            new User { Id = Guid.Parse("a0000000-0000-0000-0000-000000000003"), FirstName = "Priya",   LastName = "Nair",    Email = "priya@food.com",    PasswordHash = hash, PhoneNumber = "9000000003", Role = "Customer",        Address = "45 Indiranagar, Bengaluru",  IsActive = true, CreatedAt = now },
            new User { Id = Guid.Parse("a0000000-0000-0000-0000-000000000004"), FirstName = "Ravi",    LastName = "Kumar",   Email = "owner@food.com",    PasswordHash = hash, PhoneNumber = "9000000004", Role = "RestaurantOwner", Address = "Brigade Road, Bengaluru",    IsActive = true, CreatedAt = now },
            new User { Id = Guid.Parse("a0000000-0000-0000-0000-000000000005"), FirstName = "Arjun",   LastName = "Singh",   Email = "driver@food.com",   PasswordHash = hash, PhoneNumber = "9000000005", Role = "Driver",          Address = "HSR Layout, Bengaluru",      IsActive = true, CreatedAt = now }
        );
    }
}
