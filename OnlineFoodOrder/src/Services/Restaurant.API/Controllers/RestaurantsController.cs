using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTOs;
using Restaurant.API.Services;
using FoodOrder.Shared.Contracts;
using Restaurant.API.IServices;

namespace Restaurant.API.Controllers;

[ApiController, Route("api/restaurants")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _svc;
    public RestaurantsController(IRestaurantService svc) => _svc = svc;
    private Guid UserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var g) ? g : Guid.Empty;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? city, [FromQuery] string? cuisine)
        => Ok(ApiResponse<object>.Ok(await _svc.GetAllAsync(city, cuisine)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var r = await _svc.GetByIdAsync(id);
        return r == null ? NotFound() : Ok(ApiResponse<RestaurantDto>.Ok(r));
    }

    [Authorize(Roles = "Admin,RestaurantOwner"), HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantDto dto)
    {
        var r = await _svc.CreateAsync(dto, UserId);
        return CreatedAtAction(nameof(GetById), new { id = r.Id }, ApiResponse<RestaurantDto>.Ok(r));
    }

    [Authorize(Roles = "Admin,RestaurantOwner"), HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> ToggleOpen(Guid id)
        => await _svc.ToggleOpenAsync(id, UserId) ? Ok(ApiResponse<object>.Ok(null!)) : Forbid();

    [Authorize(Roles = "Admin,RestaurantOwner"), HttpPost("menu-items")]
    public async Task<IActionResult> AddMenuItem(CreateMenuItemDto dto)
        => Ok(ApiResponse<MenuItemDto>.Ok(await _svc.AddMenuItemAsync(dto)));

    [Authorize(Roles = "Admin,RestaurantOwner"), HttpPut("menu-items/{itemId:guid}")]
    public async Task<IActionResult> UpdateMenuItem(Guid itemId, UpdateMenuItemDto dto)
        => await _svc.UpdateMenuItemAsync(itemId, dto) ? Ok(ApiResponse<object>.Ok(null!)) : NotFound();

    [Authorize(Roles = "Admin,RestaurantOwner"), HttpPatch("menu-items/{itemId:guid}/toggle")]
    public async Task<IActionResult> ToggleItem(Guid itemId)
        => await _svc.ToggleItemAsync(itemId) ? Ok(ApiResponse<object>.Ok(null!)) : NotFound();

    [Authorize(Roles = "Admin,RestaurantOwner"), HttpPost("categories")]
    public async Task<IActionResult> AddCategory(CreateCategoryDto dto)
        => Ok(ApiResponse<CategoryDto>.Ok(await _svc.AddCategoryAsync(dto)));

    [HttpGet("menu-items/{itemId:guid}")]
    public async Task<IActionResult> GetMenuItem(Guid itemId)
    {
        var m = await _svc.GetMenuItemAsync(itemId);
        return m == null ? NotFound() : Ok(ApiResponse<MenuItemDto>.Ok(m));
    }
}
