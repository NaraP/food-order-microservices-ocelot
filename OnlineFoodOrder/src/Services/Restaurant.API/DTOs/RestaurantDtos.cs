// ── DTOs ──────────────────────────────────────────────────────────────────────
namespace Restaurant.API.DTOs;

public record RestaurantDto(Guid Id, string Name, string Description, string Address,
    string City, string Cuisine, double Rating, int DeliveryMins, decimal MinOrder,
    decimal DeliveryFee, bool IsOpen, string Phone, List<CategoryDto> Categories);

public record CategoryDto(Guid Id, string Name, int SortOrder, List<MenuItemDto> Items);

public record MenuItemDto(Guid Id, string Name, string Description, decimal Price,
    bool IsVeg, bool IsAvailable, bool IsPopular, string Tags, int PrepMins);

public record CreateRestaurantDto(string Name, string Description, string Address,
    string City, string Phone, string Cuisine, decimal MinOrder, decimal DeliveryFee, int DeliveryMins);

public record CreateMenuItemDto(string Name, string Description, decimal Price,
    bool IsVeg, string Tags, int PrepMins, Guid CategoryId);

public record UpdateMenuItemDto(string Name, string Description, decimal Price,
    bool IsVeg, bool IsAvailable, string Tags);

public record CreateCategoryDto(string Name, int SortOrder, Guid RestaurantId);
