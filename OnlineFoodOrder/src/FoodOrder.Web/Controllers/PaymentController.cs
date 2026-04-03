using FoodOrder.Web.Models;
using FoodOrder.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Web.Controllers
{

    public class PaymentController : BaseController
    {
        private readonly GatewayClient _api;
        public PaymentController(GatewayClient api) => _api = api;

        public async Task<IActionResult> History()
        {
            if (!IsAuth) return RequireAuth();
            var list = await _api.GetAsync<List<PaymentVm>>("gateway/payments/my") ?? new();
            ViewBag.User = CurrentUser;
            return View(list);
        }

        public async Task<IActionResult> Receipt(Guid orderId)
        {
            if (!IsAuth) return RequireAuth();
            var p = await _api.GetAsync<PaymentVm>($"gateway/payments/order/{orderId}");
            ViewBag.User = CurrentUser;
            return View(p);
        }
    }
}
