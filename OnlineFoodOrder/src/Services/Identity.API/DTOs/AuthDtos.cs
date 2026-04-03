// ── DTOs ──────────────────────────────────────────────────────────────────────
namespace Identity.API.DTOs;
using System.ComponentModel.DataAnnotations;

public record LoginRequest([Required][EmailAddress] string Email, [Required] string Password);
public record RegisterRequest([Required] string FirstName, [Required] string LastName,
    [Required][EmailAddress] string Email, [Required][MinLength(6)] string Password,
    [Required] string Phone, string Address = "", string Role = "Customer");
public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserInfo User);
public record UserInfo(Guid Id, string FullName, string Email, string Role, string Phone, string Address);
public record RefreshRequest([Required] string RefreshToken);
public record ChangePasswordRequest([Required] string Current, [Required][MinLength(6)] string New);
public record UpdateProfileRequest([Required] string FirstName, [Required] string LastName, string Phone, string Address);
