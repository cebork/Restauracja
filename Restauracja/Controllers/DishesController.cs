using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.ViewModels;

namespace Restauracja.Controllers
{
    public class DishesController : Controller
    {
        private readonly RestauracjaContext _context;

        public DishesController(RestauracjaContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int page = 1)
        {
            if (_context.Dish != null)
            {
                List<Dish> dishes = await _context.Dish.ToListAsync();
                int pageSize = 10;
                int totalItems = dishes.Count();
                int totalPages = (int)Math.Ceiling((double) totalItems / pageSize);
                if (page < 1)
                {
                    page = 1;
                }
                else if (page > totalPages)
                {
                    page = totalPages;
                }
                List<Dish> pagedDishes = dishes.OrderBy(i => i.DishID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new PaginationViewModel<Dish>
                {
                    Items = pagedDishes,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                };
                return View(viewModel);
            }
            else
            {
                return Problem("Entity set 'RestauracjaContext.Dish'  is null.");
            }
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishID,Name,Description,CreationDate,Price,IsAvaliable,Image")] Dish dish, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);
                byte[] data = memoryStream.ToArray();
                dish.Image = Convert.ToBase64String(data);
            }
            else
            {
                dish.Image = null;
            }
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DishID,Name,Description,CreationDate,Price,IsAvaliable,Image")] Dish dish)
        {
            if (id != dish.DishID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishID))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Dish == null)
            {
                return Problem("Entity set 'RestauracjaContext.Dish'  is null.");
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(long id)
        {
          return (_context.Dish?.Any(e => e.DishID == id)).GetValueOrDefault();
        }
    }
}
