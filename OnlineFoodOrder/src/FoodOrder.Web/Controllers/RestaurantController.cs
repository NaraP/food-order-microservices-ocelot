using FoodOrder.Web.Models;
using FoodOrder.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Web.Controllers
{
    public class RestaurantController : BaseController
    {
        private readonly GatewayClient _api;
        private readonly CartService _cart;
        public RestaurantController(GatewayClient api, CartService cart) { _api = api; _cart = cart; }

        public async Task<IActionResult> Menu(Guid id)
        {
            if (!IsAuth) return RequireAuth();
            var r = await _api.GetAsync<RestaurantVm>($"gateway/restaurants/{id}");
            if (r == null) return NotFound();
            ViewBag.User = CurrentUser;
            ViewBag.Cart = _cart.Get();
            return View(r);
        }

        [HttpPost]
        public IActionResult AddToCart(Guid restaurantId, string restaurantName, decimal deliveryFee,
            Guid itemId, string name, decimal price, bool isVeg)
        {
            _cart.Add(restaurantId, restaurantName, deliveryFee, itemId, name, price, isVeg);
            return Json(new { ok = true, count = _cart.Count });
        }

        [HttpPost]
        public IActionResult UpdateQty(Guid itemId, int qty)
        {
            _cart.UpdateQty(itemId, qty);
            var c = _cart.Get();
            return Json(new { ok = true, count = _cart.Count, total = c.Total });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid itemId)
        {
            _cart.Remove(itemId);
            return Json(new { ok = true, count = _cart.Count });
        }
    }
}
