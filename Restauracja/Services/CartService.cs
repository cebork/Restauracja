using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Linq;

namespace Restauracja.Services
{
    public interface ICartService
    {
        void AddElementToCart(long dish, int amount);
        List<Cart> getCurrentUserCart();
        void moveToOrders();
    }
    public class CartService : ICartService
    {
        private readonly RestauracjaContext _context;
        private readonly IDishService _dishService;
        private readonly IHttpContextAccessor _contextAccessor;
        public CartService(RestauracjaContext context, IDishService dishService, IHttpContextAccessor contextAccessor)
        {
            _dishService = dishService;
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async void AddElementToCart(long dish, int amount)
        {
            Dish dishObject = _context.Dish.Find(dish);
                //await _context.Dish.FirstOrDefaultAsync(m => m.DishID == dish);
            int userID = (int)_contextAccessor.HttpContext.Session.GetInt32("userID");
            User user = _context.User.Find(userID);
            Cart cart = VerifyIfDishAdded(user, dishObject);
            if (cart != null)
            {
                cart.Amount += amount;
                _context.Cart.Update(cart);
            }
            else
            {
                cart = new Cart()
                {
                    User = user,
                    Dish = dishObject,
                    Amount = amount
                };
                _context.Cart.Add(cart);
                
            }

            _context.SaveChanges();

        }

        public List<Cart> getCurrentUserCart()
        {
            int userID = (int)_contextAccessor.HttpContext.Session.GetInt32("userID");
            var restauracjaContext = _context.Cart
                .Include(c => c.Dish)
                .Include(c => c.User)
                .Where(rC => rC.User.UserId == userID);
            return restauracjaContext.ToList();
        }

        public void moveToOrders()
        {
            int userID = (int)_contextAccessor.HttpContext.Session.GetInt32("userID");
            Order order = new Order();
            order.UserId = userID;
            _context.Add(order);

            List<OrderContent> orderContents = _context.Cart.ToList().Where(c => c.UserId == userID).Select(c => new OrderContent { Amount = c.Amount, DishID = c.DishID, Order = order }).ToList();
            List<Dish> allDished = _context.Dish.ToList();
            order.FullPrice = (int)orderContents.Sum(c => c.Amount * allDished.Find(aD => aD.DishID == c.DishID).Price);
            _context.AddRange(orderContents);

            List<Cart> cartToRemove = _context.Cart.ToList().Where(c => c.UserId == userID).ToList();
            _context.RemoveRange(cartToRemove);
            _context.SaveChanges();
        }

        public Cart VerifyIfDishAdded(User user, Dish dish)
        {
            List<Cart> carts = _context.Cart.Include(c => c.Dish).Include(c => c.User).ToList();
            Cart cart = null;
            if (carts.Count > 0 && carts.Where(c => c.DishID == dish.DishID && c.UserId == user.UserId).Any())
            {
                cart = carts.Where(c => c.UserId == user.UserId && c.DishID == dish.DishID).First();
            }
                    
                
            return cart != null ? cart : null;
        }
    }
}
