namespace FoodOrder.Web.Models
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Refresh { get; set; } = string.Empty;
    }
}
