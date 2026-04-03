// ── Controller ────────────────────────────────────────────────────────────────
namespace Payment.API.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.DTOs;
using Payment.API.Services;
using FoodOrder.Shared.Contracts;
using Payment.API.IServices;

[ApiController, Route("api/payments"), Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _svc;
    public PaymentsController(IPaymentService svc) => _svc = svc;
    private Guid UserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var g) ? g : Guid.Empty;

    [HttpPost]
    public async Task<IActionResult> Initiate(InitiatePaymentDto dto)
        => Ok(ApiResponse<PaymentDto>.Ok(await _svc.InitiateAsync(dto, UserId)));

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrder(Guid orderId)
    {
        var p = await _svc.GetByOrderAsync(orderId);
        return p == null ? NotFound() : Ok(ApiResponse<PaymentDto>.Ok(p));
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyPayments()
        => Ok(ApiResponse<object>.Ok(await _svc.GetMyPaymentsAsync(UserId)));

    [HttpPost("refund"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Refund(RefundDto dto)
        => await _svc.RefundAsync(dto) ? Ok(ApiResponse<object>.Ok(null!)) : BadRequest(ApiResponse<object>.Fail("Cannot refund."));
}
