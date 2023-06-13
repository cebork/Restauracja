using System.ComponentModel.DataAnnotations;

namespace Restauracja.Models
{
    public class Dish
    {
        public long DishID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public long Price { get; set; }
        public bool IsAvaliable { get; set; }
        public string? Image { get; set; }

        public virtual Category? Category { get; set; }
        public virtual List<Ingredient>? Ingredients { get; set; }
    }
}
