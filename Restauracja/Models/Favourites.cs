
namespace Restauracja.Models
{
    
    public class Favourites
    {
        public int FavouritesID { get; set; }

        public long DishID { get; set; }
        public Dish? Dish { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
