namespace Restauracja.Models
{
    public class Ingredient
    {
        public long IngredientID { get; set; }
        public string? Name { get; set; }

        public virtual List<Dish>? Dishes { get; set; }
    }
}
