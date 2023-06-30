namespace Restauracja.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int FullPrice { get; set; }
        public bool IsDelivered { get; set; }
        public int OrderContentId { get; set; }
        public OrderContent OrderContent { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
