using Restauracja.Models;

namespace Restauracja.ViewModels
{
    public class CreateDishViewModel
    {
        public Dish Dish { get; set; }
        public CategoryViewModel? CategoryViewModel { get; set; }
        public IEnumerable<IngredientViewModel>? IngredientViewModel { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Category { get; set; }
        public List<long> SelectedIngredients { get; set; } = new List<long>();
    }
}
