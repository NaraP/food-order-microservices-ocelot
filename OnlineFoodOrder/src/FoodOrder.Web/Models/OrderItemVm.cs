namespace FoodOrder.Web.Models
{
    public class OrderItemVm
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
