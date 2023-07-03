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



        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Dish)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "DishID");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,Amount,DishID,UserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "DishID", cart.DishID);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "DishID", cart.DishID);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", cart.UserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,Amount,DishID,UserId")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "DishID", cart.DishID);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'RestauracjaContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.CartId == id)).GetValueOrDefault();
        }

        public IActionResult moveToOrders()
        {
            _cartService.moveToOrders();
            return RedirectToAction("Index", "Orders");
        }
    }
}
