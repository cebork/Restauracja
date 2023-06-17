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
using Restauracja.ViewModels;

namespace Restauracja.Controllers
{
    public class DishesController : Controller
    {
        private readonly RestauracjaContext _context;
        private readonly IDishService _dishService;
        public DishesController(RestauracjaContext context, IDishService dishService)
        {
            _context = context;
            _dishService = dishService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int page = 1)
        {
            if (_context.Dish != null)
            {
                PaginationViewModel<Dish> viewModel = await _dishService.FillPaginationViewModelAsync(page);
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

            Dish dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public async Task<IActionResult> Create()
        {
            CreateDishViewModel createDishViewModel = await _dishService.FillCreateDishViewModelForDisplayAsync();
            return View(createDishViewModel);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDishViewModel createDishViewModel)
        {

            createDishViewModel = await _dishService.FillCreateDishViewModelForSaveAsync(createDishViewModel);
            if (ModelState.IsValid)
            {
                _context.Add(createDishViewModel.Dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createDishViewModel);
        }


        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }
            Dish dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            CreateDishViewModel createDishViewModel = await _dishService.FillCreateDishViewModelForDisplayEditFormAsync(id, dish);
            return View(createDishViewModel);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateDishViewModel createDishViewModel)
        {
            createDishViewModel = await _dishService.FillCreateDishViewModelForEditAsync(createDishViewModel);
            if (ModelState.IsValid)
            {
                _context.Update(createDishViewModel.Dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createDishViewModel);
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
