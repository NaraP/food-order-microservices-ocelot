using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Delivery.API.DTOs;
using Delivery.API.Services;
using FoodOrder.Shared.Contracts;
using Delivery.API.IServices;

namespace Delivery.API.Controllers;

[ApiController, Route("api/deliveries"), Authorize]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _svc;
    public DeliveryController(IDeliveryService svc) => _svc = svc;
    private Guid UserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var g) ? g : Guid.Empty;

    [HttpPost, Authorize(Roles = "Admin,RestaurantOwner")]
    public async Task<IActionResult> Assign(AssignDto dto)
        => Ok(ApiResponse<DeliveryDto>.Ok(await _svc.AssignAsync(dto)));

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrder(Guid orderId)
    {
        var d = await _svc.GetByOrderAsync(orderId);
        return d == null ? NotFound() : Ok(ApiResponse<DeliveryDto>.Ok(d));
    }

    [HttpGet("my"), Authorize(Roles = "Driver")]
    public async Task<IActionResult> MyDeliveries()
        => Ok(ApiResponse<object>.Ok(await _svc.GetByDriverAsync(UserId)));

    [HttpGet("active"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Active()
        => Ok(ApiResponse<object>.Ok(await _svc.GetActiveAsync()));

    [HttpPatch("{id:guid}/status"), Authorize(Roles = "Driver,Admin")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto dto)
        => await _svc.UpdateStatusAsync(id, dto) ? Ok(ApiResponse<object>.Ok(null!)) : NotFound();

    [HttpPatch("{id:guid}/location"), Authorize(Roles = "Driver")]
    public async Task<IActionResult> UpdateLocation(Guid id, UpdateLocationDto dto)
        => await _svc.UpdateLocationAsync(id, dto) ? Ok(ApiResponse<object>.Ok(null!)) : NotFound();
}
