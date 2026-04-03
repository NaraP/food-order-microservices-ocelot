namespace FoodOrder.Web.Models
{
    public class RestaurantVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int DeliveryMins { get; set; }
        public decimal MinOrder { get; set; }
        public decimal DeliveryFee { get; set; }
        public bool IsOpen { get; set; }
        public string Phone { get; set; } = string.Empty;
        public List<CategoryVm> Categories { get; set; } = new();
    }
}
