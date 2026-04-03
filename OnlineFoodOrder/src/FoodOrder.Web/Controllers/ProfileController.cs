using FoodOrder.Web.Models;
using FoodOrder.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly GatewayClient _api;
        public ProfileController(GatewayClient api) => _api = api;

        public async Task<IActionResult> Index()
        {
            if (!IsAuth) return RequireAuth();
            var profile = await _api.GetAsync<ProfileVm>("gateway/auth/profile");
            ViewBag.User = CurrentUser;
            return View(profile ?? new ProfileVm
            {
                Id = CurrentUser!.Id,
                FullName = CurrentUser.FullName,
                Email = CurrentUser.Email,
                Role = CurrentUser.Role,
                Phone = CurrentUser.Phone,
                Address = CurrentUser.Address
            });
        }

        [HttpGet]
        public IActionResult Edit()
        {
            if (!IsAuth) return RequireAuth();
            var u = CurrentUser!;
            var parts = u.FullName.Split(' ', 2);
            ViewBag.User = u;
            return View(new EditProfileVm
            {
                FirstName = parts[0],
                LastName = parts.Length > 1 ? parts[1] : "",
                Phone = u.Phone,
                Address = u.Address
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileVm m)
        {
            if (!IsAuth) return RequireAuth();
            if (!ModelState.IsValid) { ViewBag.User = CurrentUser; return View(m); }
            var r = await _api.PutAsync<object>("gateway/auth/profile",
                new { m.FirstName, m.LastName, m.Phone, m.Address });
            if (r.Ok)
            {
                var u = CurrentUser!;
                u.FullName = $"{m.FirstName} {m.LastName}";
                u.Phone = m.Phone;
                u.Address = m.Address;
                SetUser(u);
                TempData["Success"] = "Profile updated.";
                return RedirectToAction("Index");
            }
            m.Error = "Update failed.";
            ViewBag.User = CurrentUser;
            return View(m);
        }

        [HttpGet]
        public IActionResult ChangePassword() { if (!IsAuth) return RequireAuth(); ViewBag.User = CurrentUser; return View(new ChangePasswordVm()); }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm m)
        {
            if (!IsAuth) return RequireAuth();
            if (!ModelState.IsValid) { ViewBag.User = CurrentUser; return View(m); }
            var r = await _api.PostAsync<object>("gateway/auth/change-password",
                new { Current = m.Current, New = m.New });
            if (r.Ok) { TempData["Success"] = "Password changed."; return RedirectToAction("Index"); }
            m.Error = "Current password is incorrect.";
            ViewBag.User = CurrentUser;
            return View(m);
        }
    }
}
