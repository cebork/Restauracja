namespace Restauracja.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Dish>? Dishes { get; set; }
    }
}
