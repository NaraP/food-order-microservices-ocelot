using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using FoodOrder.Web.Models;
using FoodOrder.Web.Services;

namespace FoodOrder.Web.Controllers;

public abstract class BaseController : Controller
{
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    protected UserSession? CurrentUser
    {
        get
        {
            var j = HttpContext.Session.GetString("User");
            return j == null ? null : JsonSerializer.Deserialize<UserSession>(j, _json);
        }
    }

    protected bool IsAuth => CurrentUser != null;
    protected string Role => CurrentUser?.Role ?? string.Empty;

    protected void SetUser(UserSession u)
    {
        HttpContext.Session.SetString("User",  JsonSerializer.Serialize(u));
        HttpContext.Session.SetString("Token", u.Token);
    }

    protected void ClearSession() => HttpContext.Session.Clear();

    protected IActionResult RequireAuth(string? returnUrl = null)
        => RedirectToAction("Login", "Auth", new { returnUrl = returnUrl ?? Request.Path.ToString() });
}

