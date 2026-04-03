using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Web.Models
{
    public class LoginVm
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        public string? Error { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
