using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Web.Models
{
    // ── Checkout ──────────────────────────────────────────────────────────────────
    public class CheckoutVm
    {
        public CartVm Cart { get; set; } = new();
        [Required] public string DeliveryAddress { get; set; } = string.Empty;
        [Required] public string PaymentMethod { get; set; } = "UPI";
        public string? Notes { get; set; }
    }
}
