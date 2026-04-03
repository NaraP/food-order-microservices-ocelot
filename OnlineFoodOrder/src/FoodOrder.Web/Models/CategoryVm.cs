namespace FoodOrder.Web.Models
{
    public class CategoryVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public List<MenuItemVm> Items { get; set; } = new();
    }
}
