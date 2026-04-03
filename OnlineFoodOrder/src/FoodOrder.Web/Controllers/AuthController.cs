using FoodOrder.Web.Helper;
using FoodOrder.Web.Models;
using FoodOrder.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly GatewayClient _api;
        public AuthController(GatewayClient api) => _api = api;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsAuth) return RedirectToAction("Index", "Home");
            return View(new LoginVm { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm m)
        {
            if (!ModelState.IsValid) return View(m);
            var r = await _api.PostAsync<AuthApiResponse>("gateway/auth/login", new { m.Email, m.Password });
            if (!r.Ok || r.Data == null) { m.Error = "Invalid email or password."; return View(m); }
            SetUser(new UserSession
            {
                Id = r.Data.User.Id,
                FullName = r.Data.User.FullName,
                Email = r.Data.User.Email,
                Role = r.Data.User.Role,
                Phone = r.Data.User.Phone,
                Address = r.Data.User.Address,
                Token = r.Data.AccessToken,
                Refresh = r.Data.RefreshToken
            });
            var ret = m.ReturnUrl;
            return !string.IsNullOrEmpty(ret) && Url.IsLocalUrl(ret) ? Redirect(ret) : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => IsAuth ? RedirectToAction("Index", "Home") : View(new RegisterVm());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm m)
        {
            if (!ModelState.IsValid) return View(m);
            var r = await _api.PostAsync<AuthApiResponse>("gateway/auth/register",
                new { m.FirstName, m.LastName, m.Email, m.Password, m.Phone, m.Address });
            if (!r.Ok || r.Data == null) { m.Error = r.Message ?? "Registration failed."; return View(m); }
            SetUser(new UserSession
            {
                Id = r.Data.User.Id,
                FullName = r.Data.User.FullName,
                Email = r.Data.User.Email,
                Role = r.Data.User.Role,
                Phone = r.Data.User.Phone,
                Address = r.Data.User.Address,
                Token = r.Data.AccessToken,
                Refresh = r.Data.RefreshToken
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout() { ClearSession(); return RedirectToAction("Login"); }

        public IActionResult AccessDenied() => View();
    }
}
