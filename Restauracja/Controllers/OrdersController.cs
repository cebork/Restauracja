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
            if (_userService.CheckIfLoggedIn())
            {
                PaginationViewModel<Order> paginationViewModel = await _orderService.FillPaginationViewModelAsync(page);
                return View(paginationViewModel);
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        public async Task<IActionResult> IndexAdmin(int page = 1)
        {
            if (_userService.CheckIfAdmin())
            {
                PaginationViewModel<Order> paginationViewModel = await _orderService.FillPaginationViewModelAdminAsync(page);
                return View(paginationViewModel);
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_userService.CheckIfLoggedIn())
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
            return RedirectToAction("AccessDenied", "Users");
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }


        public IActionResult GenerateFaktura(int id)
        {
            if (_userService.CheckIfLoggedIn())
            {
                List<OrderContent> model = _orderService.GenerateFakturaData(id);
                if (model.Count > 0)
                {
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
                }
                return NotFound();
            }
            
            return RedirectToAction("AccessDenied", "Users");
        }

        public IActionResult ChangeStatus(int id)
        {
            if (_userService.CheckIfAdmin())
            {
                _orderService.ChangeStatus(id);
                return RedirectToAction("IndexAdmin");
            }
            return RedirectToAction("AccessDenied", "Users");
        }
    }
}
