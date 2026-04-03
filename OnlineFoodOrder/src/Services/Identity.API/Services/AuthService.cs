using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Identity.API.DTOs;
using Identity.API.Entities;
using Identity.API.IServices;
using Identity.API.Repository;

namespace Identity.API.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _cfg;

    public AuthService(IUnitOfWork uow, IConfiguration cfg)
    {
        _uow = uow;
        _cfg = cfg;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        var user = await _uow.Users.GetByEmailAsync(req.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return null;

        return await IssueTokens(user);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest req)
    {
        var user = new User
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            PhoneNumber = req.Phone,
            Address = req.Address,
            Role = req.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();

        return await IssueTokens(user);
    }

    public async Task<AuthResponse?> RefreshAsync(string token)
    {
        var user = await _uow.Users.GetByRefreshTokenAsync(token);
        if (user == null) return null;

        return await IssueTokens(user);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest req)
    {
        var u = await _uow.Users.GetByIdAsync(userId);

        if (u == null || !BCrypt.Net.BCrypt.Verify(req.Current, u.PasswordHash))
            return false;

        u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.New);

        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<UserInfo?> GetProfileAsync(Guid userId)
    {
        var u = await _uow.Users.GetByIdAsync(userId);
        return u == null ? null : Map(u);
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequest req)
    {
        var u = await _uow.Users.GetByIdAsync(userId);
        if (u == null) return false;

        u.FirstName = req.FirstName;
        u.LastName = req.LastName;
        u.PhoneNumber = req.Phone;
        u.Address = req.Address;

        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task LogoutAsync(Guid userId)
    {
        var u = await _uow.Users.GetByIdAsync(userId);

        if (u != null)
        {
            u.RefreshToken = null;
            u.RefreshTokenExpiry = null;
            await _uow.SaveChangesAsync();
        }
    }

    private async Task<AuthResponse> IssueTokens(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddHours(8);

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role),
        };

        var jwt = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        var refresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        user.RefreshToken = refresh;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _uow.SaveChangesAsync();

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(jwt),
            refresh,
            expiry,
            Map(user));
    }

    private static UserInfo Map(User u) =>
        new(u.Id, u.FullName, u.Email, u.Role, u.PhoneNumber, u.Address);
}
