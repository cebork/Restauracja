using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.ViewModels;
using NuGet.Versioning;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;

namespace Restauracja.Services
{
    public interface IOrderService
    {
        Task<PaginationViewModel<Order>> FillPaginationViewModelAsync(int page);
        Task<PaginationViewModel<Order>> FillPaginationViewModelAdminAsync(int page);
        List<OrderContent> GenerateFakturaData(int id);
        void ChangeStatus(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly RestauracjaContext _context;
        private readonly IUserService _userService;


        public OrderService(RestauracjaContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public void ChangeStatus(int id)
        {
            Order order = _context.Order.Find(id);
            if (order != null)
            {
                order.IsDelivered = true;
                _context.Update(order);
                _context.SaveChanges();
            }
        }

        public async Task<PaginationViewModel<Order>> FillPaginationViewModelAdminAsync(int page)
        {
            List<Order> orders = await _context.Order
                .Include(o => o.User)
                .ToListAsync();
            int pageSize = 5;
            int totalItems = orders.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }
            List<Order> pagedOrders = orders
                .OrderByDescending(o => o.OrderId).ToList()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PaginationViewModel<Order>
            {
                Items = pagedOrders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
            return viewModel;
        }

        public async Task<PaginationViewModel<Order>> FillPaginationViewModelAsync(int page)
        {
            List<Order> orders = await _context.Order
                .Include(o => o.User)
                .ToListAsync();
            int pageSize = 5;
            int totalItems = orders.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }
            List<Order> pagedOrders = orders
                .Where(o => o.User.UserId == _userService.GetUserId())
                .OrderByDescending(o => o.OrderId).ToList()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PaginationViewModel<Order>
            {
                Items = pagedOrders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
            return viewModel;
        }

        public List<OrderContent> GenerateFakturaData(int id)
        {
            List<OrderContent> data = _context.OrderContent
                .Include(oc => oc.Dish)
                .Include(oc => oc.Order)
                .Include(oc => oc.Order.User)
                .Where(oc => oc.OrderId == id).ToList();

            return data;

        }
    }
}

