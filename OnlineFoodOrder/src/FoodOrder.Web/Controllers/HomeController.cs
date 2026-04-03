using FoodOrder.Web.Models;
using FoodOrder.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly GatewayClient _api;
        public HomeController(GatewayClient api) => _api = api;

        public async Task<IActionResult> Index(string? search, string? cuisine)
        {
            if (!IsAuth) return RedirectToAction("Login", "Auth");
            ViewBag.User = CurrentUser;

            var restaurants = await _api.GetAsync<List<RestaurantVm>>
                ($"gateway/restaurants?city=Bengaluru&cuisine={cuisine}") ?? new();

            if (!string.IsNullOrEmpty(search))
                restaurants = restaurants.Where(r =>
                    r.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    r.Cuisine.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var orders = await _api.GetAsync<List<OrderVm>>("gateway/orders/my") ?? new();

            return View(new HomeVm
            {
                Restaurants = restaurants,
                RecentOrders = orders.Take(3).ToList(),
                Cuisines = restaurants.Select(r => r.Cuisine).Distinct().OrderBy(c => c).ToList(),
                Search = search,
                Cuisine = cuisine
            });
        }

        public IActionResult Error() => View();
    }
}
