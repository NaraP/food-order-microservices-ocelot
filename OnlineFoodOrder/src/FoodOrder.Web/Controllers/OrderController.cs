using FoodOrder.Web.Models;
using FoodOrder.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly GatewayClient _api;
        private readonly CartService _cart;
        public OrderController(GatewayClient api, CartService cart) { _api = api; _cart = cart; }

        [HttpGet]
        public IActionResult Checkout()
        {
            if (!IsAuth) return RequireAuth();
            var cart = _cart.Get();
            if (!cart.Items.Any()) return RedirectToAction("Index", "Home");
            ViewBag.User = CurrentUser;
            return View(new CheckoutVm { Cart = cart, DeliveryAddress = CurrentUser?.Address ?? "" });
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutVm m)
        {
            if (!IsAuth) return RequireAuth();
            var cart = _cart.Get();
            if (!cart.Items.Any()) return RedirectToAction("Index", "Home");

            var payload = new
            {
                RestaurantId = cart.RestaurantId,
                RestaurantName = cart.RestaurantName,
                DeliveryAddress = m.DeliveryAddress,
                PaymentMethod = m.PaymentMethod,
                DeliveryFee = cart.DeliveryFee,
                Notes = m.Notes,
                Items = cart.Items.Select(i => new
                {
                    MenuItemId = i.MenuItemId,
                    Name = i.Name,
                    Quantity = i.Qty,
                    UnitPrice = i.Price,
                    Instructions = i.Note
                }).ToList()
            };

            var orderResult = await _api.PostAsync<OrderVm>("gateway/orders", payload);
            if (!orderResult.Ok || orderResult.Data == null)
            {
                TempData["Error"] = "Could not place order. Please try again.";
                ViewBag.User = CurrentUser;
                m.Cart = cart;
                return View("Checkout", m);
            }

            // Initiate payment
            await _api.PostAsync<object>("gateway/payments", new
            {
                OrderId = orderResult.Data.Id,
                Amount = orderResult.Data.TotalAmount,
                Method = m.PaymentMethod,
                CustomerEmail = CurrentUser?.Email ?? ""
            });

            _cart.Clear();
            TempData["Success"] = $"Order {orderResult.Data.OrderNumber} placed!";
            return RedirectToAction("Track", new { id = orderResult.Data.Id });
        }

        public async Task<IActionResult> Track(Guid id)
        {
            if (!IsAuth) return RequireAuth();
            var order = await _api.GetAsync<OrderVm>($"gateway/orders/{id}");
            if (order == null) return NotFound();
            var delivery = await _api.GetAsync<DeliveryVm>($"gateway/deliveries/order/{id}");
            ViewBag.User = CurrentUser;
            ViewBag.Delivery = delivery;
            return View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            if (!IsAuth) return RequireAuth();
            var orders = await _api.GetAsync<List<OrderVm>>("gateway/orders/my") ?? new();
            ViewBag.User = CurrentUser;
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id, string reason)
        {
            if (!IsAuth) return RequireAuth();
            var r = await _api.PostAsync<object>($"gateway/orders/{id}/cancel", reason);
            TempData[r.Ok ? "Success" : "Error"] = r.Ok ? "Order cancelled." : "Cannot cancel this order.";
            return RedirectToAction("MyOrders");
        }
    }
}
