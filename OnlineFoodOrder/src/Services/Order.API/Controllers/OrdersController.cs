// ── Controller ────────────────────────────────────────────────────────────────
namespace Order.API.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using FoodOrder.Shared.Contracts;
using Order.API.IServices;

[ApiController, Route("api/orders"), Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _svc;
    public OrdersController(IOrderService svc) => _svc = svc;
    private Guid   UserId    => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var g) ? g : Guid.Empty;
    private string UserName  => User.FindFirstValue(ClaimTypes.Name)  ?? string.Empty;
    private string UserEmail => User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    [HttpPost]
    public async Task<IActionResult> Place(PlaceOrderDto dto)
    {
        var order = await _svc.PlaceAsync(dto, UserId, UserName, UserEmail, string.Empty);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, ApiResponse<object>.Ok(order, "Order placed."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var o = await _svc.GetByIdAsync(id);
        return o == null ? NotFound() : Ok(ApiResponse<object>.Ok(o));
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyOrders()
        => Ok(ApiResponse<object>.Ok(await _svc.GetMyOrdersAsync(UserId)));

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<object>.Ok(await _svc.GetAllAsync()));

    [HttpGet("restaurant/{restaurantId:guid}"), Authorize(Roles = "Admin,RestaurantOwner")]
    public async Task<IActionResult> ByRestaurant(Guid restaurantId)
        => Ok(ApiResponse<object>.Ok(await _svc.GetByRestaurantAsync(restaurantId)));

    [HttpPatch("{id:guid}/status"), Authorize(Roles = "Admin,RestaurantOwner,Driver")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto dto)
    {
        var ok = await _svc.UpdateStatusAsync(id, dto);
        return ok ? Ok(ApiResponse<object>.Ok(null!)) : BadRequest(ApiResponse<object>.Fail("Cannot update status."));
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason)
    {
        var ok = await _svc.CancelAsync(id, UserId, reason);
        return ok ? Ok(ApiResponse<object>.Ok(null!)) : BadRequest(ApiResponse<object>.Fail("Cannot cancel this order."));
    }
}
