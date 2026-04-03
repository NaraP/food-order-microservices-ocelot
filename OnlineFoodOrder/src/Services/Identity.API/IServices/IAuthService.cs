using Identity.API.DTOs;

namespace Identity.API.IServices
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest req);
        Task<AuthResponse> RegisterAsync(RegisterRequest req);
        Task<AuthResponse?> RefreshAsync(string token);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest req);
        Task<UserInfo?> GetProfileAsync(Guid userId);
        Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequest req);
        Task LogoutAsync(Guid userId);
    }
}
