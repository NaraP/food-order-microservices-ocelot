using Microsoft.EntityFrameworkCore;
using Restaurant.API.Entities;

namespace Restaurant.API.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> o) : base(o) { }
    public DbSet<Entities.Restaurant> Restaurants => Set<Entities.Restaurant>();
    public DbSet<Category>  Categories => Set<Category>();
    public DbSet<MenuItem>  MenuItems  => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Entities.Restaurant>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.MinOrder).HasColumnType("decimal(10,2)");
            e.Property(r => r.DeliveryFee).HasColumnType("decimal(10,2)");
        });
        b.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasOne(c => c.Restaurant).WithMany(r => r.Categories)
             .HasForeignKey(c => c.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        });
        b.Entity<MenuItem>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Price).HasColumnType("decimal(10,2)");
            e.HasOne(m => m.Category).WithMany(c => c.MenuItems)
             .HasForeignKey(m => m.CategoryId).OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(b);
    }

    private static void SeedData(ModelBuilder b)
    {
        var owner = Guid.Parse("a0000000-0000-0000-0000-000000000004");
        var now   = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Restaurants
        var r1 = Guid.Parse("b1000000-0000-0000-0000-000000000001");
        var r2 = Guid.Parse("b1000000-0000-0000-0000-000000000002");
        var r3 = Guid.Parse("b1000000-0000-0000-0000-000000000003");

        b.Entity<Entities.Restaurant>().HasData(
            new Entities.Restaurant { Id=r1, Name="Spice Garden",   Description="Authentic South Indian cuisine",           Address="12 Brigade Road", City="Bengaluru", Phone="080-1111111", Cuisine="South Indian", Rating=4.6, DeliveryMins=35, MinOrder=150, DeliveryFee=25, IsOpen=true, IsActive=true, OwnerId=owner, CreatedAt=now },
            new Entities.Restaurant { Id=r2, Name="Pizza Hub",       Description="Stone-baked pizzas & Italian favourites",  Address="45 Koramangala",  City="Bengaluru", Phone="080-2222222", Cuisine="Italian",      Rating=4.4, DeliveryMins=40, MinOrder=200, DeliveryFee=30, IsOpen=true, IsActive=true, OwnerId=owner, CreatedAt=now },
            new Entities.Restaurant { Id=r3, Name="Biryani Palace",  Description="Dum biryani specialists since 2010",       Address="78 Indiranagar",  City="Bengaluru", Phone="080-3333333", Cuisine="Mughlai",      Rating=4.8, DeliveryMins=45, MinOrder=250, DeliveryFee=35, IsOpen=true, IsActive=true, OwnerId=owner, CreatedAt=now }
        );

        // Categories
        var c1=Guid.Parse("c1000000-0000-0000-0000-000000000001"); var c2=Guid.Parse("c1000000-0000-0000-0000-000000000002");
        var c3=Guid.Parse("c1000000-0000-0000-0000-000000000003"); var c4=Guid.Parse("c1000000-0000-0000-0000-000000000004");
        var c5=Guid.Parse("c1000000-0000-0000-0000-000000000005"); var c6=Guid.Parse("c1000000-0000-0000-0000-000000000006");

        b.Entity<Category>().HasData(
            new Category { Id=c1, Name="Starters",    SortOrder=1, RestaurantId=r1, IsActive=true },
            new Category { Id=c2, Name="Main Course",  SortOrder=2, RestaurantId=r1, IsActive=true },
            new Category { Id=c3, Name="Pizzas",       SortOrder=1, RestaurantId=r2, IsActive=true },
            new Category { Id=c4, Name="Pasta",        SortOrder=2, RestaurantId=r2, IsActive=true },
            new Category { Id=c5, Name="Biryani",      SortOrder=1, RestaurantId=r3, IsActive=true },
            new Category { Id=c6, Name="Curries",      SortOrder=2, RestaurantId=r3, IsActive=true }
        );

        // Menu Items
        b.Entity<MenuItem>().HasData(
            // Spice Garden – Starters
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000001"), Name="Masala Dosa",           Description="Crispy rice crepe with spiced potato filling, sambar & chutneys",    Price=120, IsVeg=true,  IsPopular=true,  Tags="bestseller,veg",  PrepMins=12, CategoryId=c1, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000002"), Name="Medu Vada",             Description="Crispy lentil donuts with sambar and coconut chutney",              Price=80,  IsVeg=true,  IsPopular=false, Tags="veg",             PrepMins=10, CategoryId=c1, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000003"), Name="Chicken 65",            Description="Spicy deep-fried chicken — a South Indian classic",                Price=220, IsVeg=false, IsPopular=true,  Tags="spicy,bestseller",PrepMins=18, CategoryId=c1, IsActive=true, CreatedAt=now },
            // Spice Garden – Main Course
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000004"), Name="Ghee Rice + Dal Fry",   Description="Fragrant ghee rice with dal fry, pickle & papad",                  Price=180, IsVeg=true,  IsPopular=false, Tags="veg",             PrepMins=20, CategoryId=c2, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000005"), Name="Chettinad Chicken",     Description="Bold & aromatic chicken curry with freshly ground spices",         Price=300, IsVeg=false, IsPopular=true,  Tags="spicy",           PrepMins=25, CategoryId=c2, IsActive=true, CreatedAt=now },
            // Pizza Hub – Pizzas
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000006"), Name="Margherita",            Description="Classic tomato sauce, fresh mozzarella, basil",                   Price=350, IsVeg=true,  IsPopular=true,  Tags="veg,classic",     PrepMins=20, CategoryId=c3, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000007"), Name="BBQ Chicken Pizza",     Description="Smoky BBQ sauce, grilled chicken, caramelised onions",             Price=450, IsVeg=false, IsPopular=true,  Tags="bestseller",      PrepMins=22, CategoryId=c3, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000008"), Name="Veggie Supreme",        Description="Bell peppers, olives, mushrooms, corn on a crispy base",           Price=380, IsVeg=true,  IsPopular=false, Tags="veg",             PrepMins=20, CategoryId=c3, IsActive=true, CreatedAt=now },
            // Pizza Hub – Pasta
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000009"), Name="Pasta Arrabiata",       Description="Penne in spicy tomato sauce with garlic & herbs",                  Price=280, IsVeg=true,  IsPopular=false, Tags="spicy,veg",       PrepMins=15, CategoryId=c4, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000010"), Name="Chicken Alfredo",       Description="Fettuccine in creamy Alfredo sauce with grilled chicken",          Price=340, IsVeg=false, IsPopular=true,  Tags="creamy",          PrepMins=18, CategoryId=c4, IsActive=true, CreatedAt=now },
            // Biryani Palace – Biryani
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000011"), Name="Hyderabadi Dum Biryani",Description="Slow-cooked biryani with saffron, caramelised onions & raita",    Price=320, IsVeg=false, IsPopular=true,  Tags="bestseller,dum",  PrepMins=35, CategoryId=c5, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000012"), Name="Veg Dum Biryani",       Description="Aromatic basmati with mixed vegetables, mint & whole spices",     Price=240, IsVeg=true,  IsPopular=false, Tags="veg,dum",         PrepMins=35, CategoryId=c5, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000013"), Name="Mutton Biryani",        Description="Tender slow-cooked mutton in long-grain basmati",                  Price=420, IsVeg=false, IsPopular=true,  Tags="premium,dum",     PrepMins=45, CategoryId=c5, IsActive=true, CreatedAt=now },
            // Biryani Palace – Curries
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000014"), Name="Butter Chicken",        Description="Creamy tomato-based chicken curry — a timeless classic",          Price=280, IsVeg=false, IsPopular=true,  Tags="mild,bestseller", PrepMins=25, CategoryId=c6, IsActive=true, CreatedAt=now },
            new MenuItem { Id=Guid.Parse("d1000000-0000-0000-0000-000000000015"), Name="Paneer Butter Masala",  Description="Soft paneer in rich, buttery tomato gravy",                       Price=260, IsVeg=true,  IsPopular=true,  Tags="veg,mild",        PrepMins=20, CategoryId=c6, IsActive=true, CreatedAt=now }
        );
    }
}
