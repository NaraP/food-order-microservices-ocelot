namespace FoodOrder.Web.Models
{
    public class HomeVm
    {
        public List<RestaurantVm> Restaurants { get; set; } = new();
        public List<OrderVm> RecentOrders { get; set; } = new();
        public List<string> Cuisines { get; set; } = new();
        public string? Search { get; set; }
        public string? Cuisine { get; set; }
    }
}
