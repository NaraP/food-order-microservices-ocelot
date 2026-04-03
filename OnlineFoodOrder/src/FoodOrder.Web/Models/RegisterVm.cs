using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Web.Models
{
    public class RegisterVm
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
