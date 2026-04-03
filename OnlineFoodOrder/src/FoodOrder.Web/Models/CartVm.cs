namespace FoodOrder.Web.Models
{
    public class CartVm
    {
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; }
        public List<CartItemVm> Items { get; set; } = new();
        public decimal SubTotal => Items.Sum(i => i.LineTotal);
        public decimal Tax => Math.Round(SubTotal * 0.05m, 2);
        public decimal Total => SubTotal + DeliveryFee + Tax;
        public int Count => Items.Sum(i => i.Qty);
    }
}
