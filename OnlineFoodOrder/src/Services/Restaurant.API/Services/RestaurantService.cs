// ── Service ───────────────────────────────────────────────────────────────────
namespace Restaurant.API.Services;
using Restaurant.API.DTOs;
using Restaurant.API.Entities;
using Restaurant.API.IServices;
using Restaurant.API.Repository;

public class RestaurantService : IRestaurantService
{
    private readonly IUnitOfWork _uow;

    public RestaurantService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllAsync(string? city, string? cuisine)
    {
        var list = await _uow.Restaurants.GetAllWithMenuAsync(city, cuisine);
        return list.Select(ToDto);
    }

    public async Task<RestaurantDto?> GetByIdAsync(Guid id)
    {
        var r = await _uow.Restaurants.GetByIdWithMenuAsync(id);
        return r == null ? null : ToDto(r);
    }

    public async Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto, Guid ownerId)
    {
        var r = new Entities.Restaurant
        {
            Name = dto.Name,
            Description = dto.Description,
            Address = dto.Address,
            City = dto.City,
            Phone = dto.Phone,
            Cuisine = dto.Cuisine,
            MinOrder = dto.MinOrder,
            DeliveryFee = dto.DeliveryFee,
            DeliveryMins = dto.DeliveryMins,
            OwnerId = ownerId
        };

        await _uow.Restaurants.AddAsync(r);
        await _uow.SaveChangesAsync();

        var created = await _uow.Restaurants.GetByIdWithMenuAsync(r.Id);
        return ToDto(created!);
    }

    public async Task<bool> ToggleOpenAsync(Guid id, Guid ownerId)
    {
        var r = await _uow.Restaurants.FindAsync(x => x.Id == id && x.OwnerId == ownerId);
        var entity = r.FirstOrDefault();

        if (entity == null) return false;

        entity.IsOpen = !entity.IsOpen;
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<MenuItemDto> AddMenuItemAsync(CreateMenuItemDto dto)
    {
        var m = new MenuItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            IsVeg = dto.IsVeg,
            Tags = dto.Tags,
            PrepMins = dto.PrepMins,
            CategoryId = dto.CategoryId
        };

        await _uow.MenuItems.AddAsync(m);
        await _uow.SaveChangesAsync();

        return ToItemDto(m);
    }

    public async Task<bool> UpdateMenuItemAsync(Guid itemId, UpdateMenuItemDto dto)
    {
        var m = await _uow.MenuItems.GetByIdAsync(itemId);
        if (m == null) return false;

        m.Name = dto.Name;
        m.Description = dto.Description;
        m.Price = dto.Price;
        m.IsVeg = dto.IsVeg;
        m.IsAvailable = dto.IsAvailable;
        m.Tags = dto.Tags;

        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleItemAsync(Guid itemId)
    {
        var m = await _uow.MenuItems.GetByIdAsync(itemId);
        if (m == null) return false;

        m.IsAvailable = !m.IsAvailable;
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto)
    {
        var c = new Category
        {
            Name = dto.Name,
            SortOrder = dto.SortOrder,
            RestaurantId = dto.RestaurantId
        };

        await _uow.Categories.AddAsync(c);
        await _uow.SaveChangesAsync();

        return new CategoryDto(c.Id, c.Name, c.SortOrder, new());
    }

    public async Task<MenuItemDto?> GetMenuItemAsync(Guid itemId)
    {
        var m = await _uow.MenuItems.GetByIdAsync(itemId);
        return m == null ? null : ToItemDto(m);
    }

    // Mapping stays same
    private static RestaurantDto ToDto(Entities.Restaurant r) => new(
        r.Id, r.Name, r.Description, r.Address, r.City, r.Cuisine, r.Rating,
        r.DeliveryMins, r.MinOrder, r.DeliveryFee, r.IsOpen, r.Phone,
        r.Categories.Where(c => c.IsActive).OrderBy(c => c.SortOrder)
            .Select(c => new CategoryDto(c.Id, c.Name, c.SortOrder,
                c.MenuItems.Where(m => m.IsActive && m.IsAvailable)
                           .Select(ToItemDto).ToList()))
            .ToList());

    private static MenuItemDto ToItemDto(MenuItem m) =>
        new(m.Id, m.Name, m.Description, m.Price, m.IsVeg, m.IsAvailable, m.IsPopular, m.Tags, m.PrepMins);
}