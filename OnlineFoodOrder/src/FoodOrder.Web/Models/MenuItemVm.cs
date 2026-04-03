namespace FoodOrder.Web.Models
{
    public class MenuItemVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsVeg { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPopular { get; set; }
        public string Tags { get; set; } = string.Empty;
        public int PrepMins { get; set; }
    }
}
