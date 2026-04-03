using FoodOrder.Web.Controllers;

namespace FoodOrder.Web.Helper
{
    public class AuthApiResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserApiInfo User { get; set; } = null!;
    }
}
