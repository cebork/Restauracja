namespace Restauracja.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int FullPrice { get; set; } = 0;
        public bool IsDelivered { get; set; } = false;
        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<OrderContent>? OrderContents { get; set; }

    }
}
