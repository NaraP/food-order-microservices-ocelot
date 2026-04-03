using Restaurant.API.DTOs;

namespace Restaurant.API.IServices
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantDto>> GetAllAsync(string? city, string? cuisine);
        Task<RestaurantDto?> GetByIdAsync(Guid id);
        Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto, Guid ownerId);
        Task<bool> ToggleOpenAsync(Guid id, Guid ownerId);
        Task<MenuItemDto> AddMenuItemAsync(CreateMenuItemDto dto);
        Task<bool> UpdateMenuItemAsync(Guid itemId, UpdateMenuItemDto dto);
        Task<bool> ToggleItemAsync(Guid itemId);
        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto);
        Task<MenuItemDto?> GetMenuItemAsync(Guid itemId);
    }
}
