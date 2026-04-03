using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.API.DTOs;
using Identity.API.Services;
using FoodOrder.Shared.Contracts;
using Identity.API.IServices;

namespace Identity.API.Controllers;

[ApiController, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    private Guid CurrentId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var g) ? g : Guid.Empty;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var result = await _auth.RegisterAsync(req);
        return Ok(ApiResponse<AuthResponse>.Ok(result, "Registration successful."));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _auth.LoginAsync(req);
        if (result == null) return Unauthorized(ApiResponse<AuthResponse>.Fail("Invalid email or password."));
        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest req)
    {
        var result = await _auth.RefreshAsync(req.RefreshToken);
        if (result == null) return Unauthorized(ApiResponse<AuthResponse>.Fail("Invalid or expired token."));
        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [Authorize, HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _auth.LogoutAsync(CurrentId);
        return Ok(ApiResponse<object>.Ok(null!, "Logged out."));
    }

    [Authorize, HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var u = await _auth.GetProfileAsync(CurrentId);
        return u == null ? NotFound() : Ok(ApiResponse<UserInfo>.Ok(u));
    }

    [Authorize, HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest req)
    {
        var ok = await _auth.UpdateProfileAsync(CurrentId, req);
        return ok ? Ok(ApiResponse<object>.Ok(null!)) : NotFound();
    }

    [Authorize, HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
    {
        var ok = await _auth.ChangePasswordAsync(CurrentId, req);
        return ok ? Ok(ApiResponse<object>.Ok(null!)) : BadRequest(ApiResponse<object>.Fail("Current password incorrect."));
    }

    [Authorize, HttpGet("validate")]
    public IActionResult Validate() => Ok(new
    {
        UserId = CurrentId,
        Email  = User.FindFirstValue(ClaimTypes.Email),
        Role   = User.FindFirstValue(ClaimTypes.Role)
    });
}
