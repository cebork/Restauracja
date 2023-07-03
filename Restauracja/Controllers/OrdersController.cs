using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.Services;
using Restauracja.ViewModels;

namespace Restauracja.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RestauracjaContext _context;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public OrdersController(RestauracjaContext context, IUserService userService, IOrderService orderService, ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _context = context;
            _userService = userService;
            _orderService = orderService;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int page = 1)
        {
            PaginationViewModel<Order> paginationViewModel = await _orderService.FillPaginationViewModelAsync(page);
            return View(paginationViewModel);
        }

        public async Task<IActionResult> IndexAdmin(int page = 1)
        {
            PaginationViewModel<Order> paginationViewModel = await _orderService.FillPaginationViewModelAdminAsync(page);
            return View(paginationViewModel);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = _orderService.GenerateFakturaData((int)id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,FullPrice,IsDelivered,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,FullPrice,IsDelivered,UserId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'RestauracjaContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }


        public IActionResult GenerateFaktura(int id)
        {

            List<OrderContent> model = _orderService.GenerateFakturaData(id);
            var viewResult = _viewEngine.FindView(ControllerContext, "GenerateFaktura", false);
            if (viewResult.Success) 
            {
                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                using (var writer = new StringWriter())
                {
                    var viewContext = new ViewContext(
                        ControllerContext,
                        viewResult.View,
                        viewData,
                        new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                        writer,
                        new HtmlHelperOptions()
                    );

                    viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();
                    writer.Flush();

                    var htmlContent = writer.ToString();

                    var renderer = new ChromePdfRenderer();
                    var pdf = renderer.RenderHtmlAsPdf(htmlContent);

                    return File(pdf.BinaryData, "application/pdf", $"Faktura{model[0].OrderId}.pdf");

                }
            }
            return NotFound();
        }

        public IActionResult ChangeStatus(int id)
        {
            _orderService.ChangeStatus(id);
            return RedirectToAction("IndexAdmin");
        }
    }
}
