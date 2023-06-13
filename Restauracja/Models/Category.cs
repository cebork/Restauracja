namespace Restauracja.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Dish>? Dishes { get; set; }
    }
}
