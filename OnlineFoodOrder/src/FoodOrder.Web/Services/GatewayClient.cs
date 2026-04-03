using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoodOrder.Web.Services;

public class GatewayClient
{
    private readonly HttpClient           _http;
    private readonly IHttpContextAccessor _ctx;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public GatewayClient(HttpClient http, IHttpContextAccessor ctx)
    {
        _http = http;
        _ctx  = ctx;
    }

    // ── Token attachment ──────────────────────────────────────────────────────
    private void Attach()
    {
        var token = _ctx.HttpContext?.Session.GetString("Token");
        _http.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    // ── GET ───────────────────────────────────────────────────────────────────
    public async Task<T?> GetAsync<T>(string url)
    {
        Attach();
        try
        {
            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return default;
            var body = await resp.Content.ReadAsStringAsync();
            var wrap = JsonSerializer.Deserialize<Wrap<T>>(body, _json);
            return wrap!.Data;
        }
        catch { return default; }
    }

    // ── POST ──────────────────────────────────────────────────────────────────
    public async Task<Result<T>> PostAsync<T>(string url, object body)
    {
        Attach();
        try
        {
            var json = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync(url, json);
            var raw  = await resp.Content.ReadAsStringAsync();
            return new Result<T>(resp.IsSuccessStatusCode, (int)resp.StatusCode,
                TryParse<Wrap<T>>(raw)!.Data, TryParse<Wrap<T>>(raw)?.Message);
        }
        catch (Exception ex)
        {
            return new Result<T>(false, 0, default, ex.Message);
        }
    }

    // ── PUT ───────────────────────────────────────────────────────────────────
    public async Task<Result<T>> PutAsync<T>(string url, object body)
    {
        Attach();
        try
        {
            var json = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var resp = await _http.PutAsync(url, json);
            var raw  = await resp.Content.ReadAsStringAsync();
            return new Result<T>(resp.IsSuccessStatusCode, (int)resp.StatusCode,
                TryParse<Wrap<T>>(raw)!.Data, TryParse<Wrap<T>>(raw)?.Message);
        }
        catch (Exception ex)
        {
            return new Result<T>(false, 0, default, ex.Message);
        }
    }

    // ── PATCH ─────────────────────────────────────────────────────────────────
    public async Task<bool> PatchAsync(string url, object body)
    {
        Attach();
        var json = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var req  = new HttpRequestMessage(HttpMethod.Patch, url) { Content = json };
        var resp = await _http.SendAsync(req);
        return resp.IsSuccessStatusCode;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    private T? TryParse<T>(string json)
    {
        try { return JsonSerializer.Deserialize<T>(json, _json); }
        catch { return default; }
    }

    private class Wrap<T>
    {
        public bool    Success { get; set; }
        public string? Message { get; set; }
        public T?      Data    { get; set; }
    }
}

public record Result<T>(bool Ok, int Status, T? Data, string? Message);

// ── Session-based Cart ────────────────────────────────────────────────────────
public class CartService
{
    private const string Key = "Cart";
    private readonly IHttpContextAccessor _ctx;
    public CartService(IHttpContextAccessor ctx) => _ctx = ctx;

    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public FoodOrder.Web.Models.CartVm Get()
    {
        var json = _ctx.HttpContext?.Session.GetString(Key);
        return json == null
            ? new()
            : JsonSerializer.Deserialize<FoodOrder.Web.Models.CartVm>(json, _json) ?? new();
    }

    public void Save(FoodOrder.Web.Models.CartVm cart)
        => _ctx.HttpContext?.Session.SetString(Key, JsonSerializer.Serialize(cart));

    public void Add(Guid restaurantId, string restaurantName, decimal deliveryFee,
        Guid itemId, string name, decimal price, bool isVeg, int qty = 1)
    {
        var cart = Get();
        if (cart.Items.Any() && cart.RestaurantId != restaurantId)
            cart = new();
        cart.RestaurantId   = restaurantId;
        cart.RestaurantName = restaurantName;
        cart.DeliveryFee    = deliveryFee;
        var existing = cart.Items.FirstOrDefault(i => i.MenuItemId == itemId);
        if (existing != null) existing.Qty += qty;
        else cart.Items.Add(new() { MenuItemId=itemId, Name=name, Price=price, Qty=qty, IsVeg=isVeg });
        Save(cart);
    }

    public void UpdateQty(Guid itemId, int qty)
    {
        var cart = Get();
        var item = cart.Items.FirstOrDefault(i => i.MenuItemId == itemId);
        if (item != null) { item.Qty = qty; if (item.Qty <= 0) cart.Items.Remove(item); }
        Save(cart);
    }

    public void Remove(Guid itemId)
    {
        var cart = Get();
        cart.Items.RemoveAll(i => i.MenuItemId == itemId);
        Save(cart);
    }

    public void Clear() => _ctx.HttpContext?.Session.Remove(Key);
    public int Count => Get().Count;
}
