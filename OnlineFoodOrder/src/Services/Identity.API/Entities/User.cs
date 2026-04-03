// ── Entity ────────────────────────────────────────────────────────────────────
namespace Identity.API.Entities;

public class User
{
    public Guid     Id           { get; set; } = Guid.NewGuid();
    public string   FirstName    { get; set; } = string.Empty;
    public string   LastName     { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   PasswordHash { get; set; } = string.Empty;
    public string   PhoneNumber       { get; set; } = string.Empty;
    public string   Address      { get; set; } = string.Empty;
    public string   Role         { get; set; } = "Customer";   // Customer | Admin | RestaurantOwner | Driver
    public bool     IsActive     { get; set; } = true;
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;
    public string?  RefreshToken       { get; set; }
    public DateTime? RefreshTokenExpiry{ get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
