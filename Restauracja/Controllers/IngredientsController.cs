using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.Services;

namespace Restauracja.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly RestauracjaContext _context;
        private readonly IUserService _userService;
        public IngredientsController(RestauracjaContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            if (_userService.CheckIfAdmin())
            {
                return _context.Ingredient != null ?
                          View(await _context.Ingredient.ToListAsync()) :
                          Problem("Entity set 'RestauracjaContext.Ingredient'  is null.");
            }
              

            return RedirectToAction("AccessDenied", "Users");
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            if (_userService.CheckIfAdmin())
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientID,Name,IsDeleted")] Ingredient ingredient)
        {
            if (_userService.CheckIfAdmin())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ingredient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(ingredient);
            }
            return RedirectToAction("AccessDenied", "Users");
        }


        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (_userService.CheckIfAdmin())
            {
                if (id == null || _context.Ingredient == null)
                {
                    return NotFound();
                }

                var ingredient = await _context.Ingredient
                    .FirstOrDefaultAsync(m => m.IngredientID == id);
                if (ingredient == null)
                {
                    return NotFound();
                }

                return View(ingredient);
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_userService.CheckIfAdmin())
            {
                if (_context.Ingredient == null)
                {
                    return Problem("Entity set 'RestauracjaContext.Ingredient'  is null.");
                }
                var ingredient = await _context.Ingredient.FindAsync(id);
                if (ingredient != null)
                {
                    _context.Ingredient.Remove(ingredient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        private bool IngredientExists(long id)
        {
          return (_context.Ingredient?.Any(e => e.IngredientID == id)).GetValueOrDefault();
        }
    }
}
