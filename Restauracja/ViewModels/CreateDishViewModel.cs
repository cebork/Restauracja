using Restauracja.Models;

namespace Restauracja.ViewModels
{
    public class CreateDishViewModel
    {
        public Dish Dish { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }
        public IEnumerable<IngredientViewModel> IngredientViewModel { get; set; }
    }
}
