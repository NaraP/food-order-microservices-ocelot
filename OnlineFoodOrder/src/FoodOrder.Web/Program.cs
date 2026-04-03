using FoodOrder.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(o =>
{
    o.IdleTimeout      = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly  = true;
    o.Cookie.IsEssential = true;
    o.Cookie.Name      = ".FoodOrder.Session";
});

// Typed HttpClient pointing at the Gateway
builder.Services.AddHttpClient<GatewayClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["GatewayUrl"]!);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<CartService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
