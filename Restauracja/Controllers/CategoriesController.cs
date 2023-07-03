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
    public class CategoriesController : Controller
    {
        private readonly RestauracjaContext _context;
        private readonly IUserService _userService;
        public CategoriesController(RestauracjaContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (_userService.CheckIfAdmin())
            {
                var categoriesAll = await _context.Category.ToListAsync();
                List<Category> categoriesListNotDeleted = categoriesAll.Where(c => c.IsDeleted == false).ToList();
                return _context.Category != null ?
                            View(categoriesListNotDeleted) :
                            Problem("Entity set 'RestauracjaContext.Category'  is null.");
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            if (_userService.CheckIfAdmin())
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,Name")] Category category)
        {
            if(_userService.CheckIfAdmin())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_userService.CheckIfAdmin())
            {
                if (id == null || _context.Category == null)
                {
                    return NotFound();
                }

                var category = await _context.Category
                    .FirstOrDefaultAsync(m => m.CategoryID == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_userService.CheckIfAdmin())
            {
                if (_context.Category == null)
                {
                    return Problem("Entity set 'RestauracjaContext.Category'  is null.");
                }
                var category = await _context.Category.FindAsync(id);
                if (category != null)
                {
                    category.IsDeleted = true;
                    _context.Category.Update(category);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.CategoryID == id)).GetValueOrDefault();
        }
    }
}
