using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.Services;
using Restauracja.ViewModels;

namespace Restauracja.Controllers
{
    public class CartsController : Controller
    {
        private readonly RestauracjaContext _context;
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        public CartsController(RestauracjaContext context, ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _context = context;
            _userService = userService;
        }


        // GET: Carts
        public async Task<IActionResult> Index()
        {
            if (_userService.CheckIfLoggedIn())
            {
                return View(_cartService.getCurrentUserCart().ToList());
            }
            
            return RedirectToAction("AccessDenied", "Users");
        }


        [HttpPost]
        public IActionResult ToCart(long dish, int amount)
        {
            if (_userService.CheckIfLoggedIn())
            {
                _cartService.AddElementToCart(dish, amount);
                return RedirectToAction("Index", "Carts");
            }
            return RedirectToAction("AccessDenied", "Users");
        }


       

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_userService.CheckIfLoggedIn())
            {
                if (id == null || _context.Cart == null)
                {
                    return NotFound();
                }

                var cart = await _context.Cart
                    .Include(c => c.Dish)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(m => m.CartId == id);
                if (cart != null)
                {
                    _context.Cart.Remove(cart);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.CartId == id)).GetValueOrDefault();
        }

        public IActionResult moveToOrders()
        {
            if (_userService.CheckIfLoggedIn())
            {
                _cartService.moveToOrders();
                return RedirectToAction("Index", "Orders");
            }
            return RedirectToAction("AccessDenied", "Users");
        }
    }
}
