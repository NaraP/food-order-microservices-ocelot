using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Web.Models
{
    public class EditProfileVm
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
