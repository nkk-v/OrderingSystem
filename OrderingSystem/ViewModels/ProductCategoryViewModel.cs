using OrderingSystem.Models;

namespace OrderingSystem.ViewModels
{
    public class ProductCategoryViewModel
    {
        public List<Category> categories  { get; set; }
        public List<Product> products { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
