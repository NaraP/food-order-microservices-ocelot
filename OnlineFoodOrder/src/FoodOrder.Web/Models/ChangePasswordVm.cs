using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Web.Models
{
    public class ChangePasswordVm
    {
        [Required] public string Current { get; set; } = string.Empty;
        [Required, MinLength(6)] public string New { get; set; } = string.Empty;
        [Compare("New")] public string Confirm { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
