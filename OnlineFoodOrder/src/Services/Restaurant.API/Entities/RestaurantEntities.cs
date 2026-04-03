namespace Restaurant.API.Entities;

public class Restaurant
{
    public Guid     Id           { get; set; } = Guid.NewGuid();
    public string   Name         { get; set; } = string.Empty;
    public string   Description  { get; set; } = string.Empty;
    public string   Address      { get; set; } = string.Empty;
    public string   City         { get; set; } = string.Empty;
    public string   Phone        { get; set; } = string.Empty;
    public string   Cuisine      { get; set; } = string.Empty;
    public double   Rating       { get; set; }
    public int      DeliveryMins { get; set; } = 30;
    public decimal  MinOrder     { get; set; }
    public decimal  DeliveryFee  { get; set; }
    public bool     IsOpen       { get; set; } = true;
    public bool     IsActive     { get; set; } = true;
    public Guid     OwnerId      { get; set; }
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}

public class Category
{
    public Guid     Id           { get; set; } = Guid.NewGuid();
    public string   Name         { get; set; } = string.Empty;
    public int      SortOrder    { get; set; }
    public bool     IsActive     { get; set; } = true;
    public Guid     RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}

public class MenuItem
{
    public Guid     Id          { get; set; } = Guid.NewGuid();
    public string   Name        { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public decimal  Price       { get; set; }
    public bool     IsVeg       { get; set; }
    public bool     IsAvailable { get; set; } = true;
    public bool     IsPopular   { get; set; }
    public bool     IsActive    { get; set; } = true;
    public string   Tags        { get; set; } = string.Empty;
    public int      PrepMins    { get; set; } = 15;
    public Guid     CategoryId  { get; set; }
    public Category Category    { get; set; } = null!;
    public DateTime CreatedAt   { get; set; } = DateTime.UtcNow;
}
