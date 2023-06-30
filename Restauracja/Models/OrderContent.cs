namespace Restauracja.Models
{
    public class OrderContent
    {
        public int OrderContentId { get; set; }
        public int Amount { get; set; }

        public long DishID { get; set; }
        public Dish Dish { get; set; }


        public virtual ICollection<Order>? Orders { get; set; }
    }
}
