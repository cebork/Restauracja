namespace Restauracja.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public User User { get; set; }
        public Dish Dish { get; set; }
        public int Amount { get; set; }
    }
}
