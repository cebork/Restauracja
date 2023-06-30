namespace Restauracja.Models
{
    public class Ingredient
    {
        public long IngredientID { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Dish> Dishes { get; set;  } = new();
        public override bool Equals(object? obj)
        {
            return ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string? ToString()
        {
            return $"INGREDIENT {IngredientID}, {Name}";
        }
    }
}
