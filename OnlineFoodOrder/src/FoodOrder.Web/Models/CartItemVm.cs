namespace FoodOrder.Web.Models
{
    public class CartItemVm
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public bool IsVeg { get; set; }
        public string? Note { get; set; }
        public decimal LineTotal => Price * Qty;
    }
}
