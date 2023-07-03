using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.ViewModels;
using System;

namespace Restauracja.Services
{
    public interface IUserService
    {
        bool checkIfSessionIsSet();
        bool CheckIfLoggedIn();
        bool CheckIfAdmin();
        Task<PaginationViewModel<User>> FillPaginationViewModelAsync(int page, string searchString);
        void ActivateOrDeactivateUser(int userID);
        void ChangeRole(int userID);
        int GetUserId();
    }
    public class UserService : IUserService
    {
        private readonly RestauracjaContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(RestauracjaContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public bool checkIfSessionIsSet()
        {
            return _contextAccessor.HttpContext.Session.Keys.Any();
        }

        public bool CheckIfLoggedIn()
        {
            return checkIfSessionIsSet()
                && _contextAccessor.HttpContext.Session.TryGetValue("role", out byte[] sessionBytes1)
                && _contextAccessor.HttpContext.Session.TryGetValue("firstName", out byte[] sessionBytes2)
                && _contextAccessor.HttpContext.Session.TryGetValue("lastName", out byte[] sessionBytes3) ? true : false;
        }

        public bool CheckIfAdmin()
        {
            return CheckIfLoggedIn()
                && _contextAccessor.HttpContext.Session.GetString("role") == "Admin";
        }

        public async Task<PaginationViewModel<User>> FillPaginationViewModelAsync(int page, string searchString)
        {
            List<User> users = await _context.User
                .Include(u => u.Role)
                .ToListAsync();
            int pageSize = 5;
            int totalItems = users.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            List<User> pagedUsers;
            if (searchString != null)
            {
                pagedUsers = users.OrderBy(i => i.UserId).Where(d => d.Email.Contains(searchString) || d.FirstName.Contains(searchString) || d.LastName.Contains(searchString) || d.City.Contains(searchString))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                pagedUsers = users.OrderBy(i => i.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }

            var viewModel = new PaginationViewModel<User>
            {
                Items = pagedUsers,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
            return viewModel;
        }

        public void ActivateOrDeactivateUser(int userID)
        {
            User user = _context.User.Find(userID);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                _context.Update(user);
                _context.SaveChanges();
            }
        }

        public void ChangeRole(int userID)
        {
            User user = _context.User.Find(userID);
            if (user != null)
            {
                user.RoleId = user.RoleId == 1 ? 2 : 1;
                _context.Update(user);
                _context.SaveChanges();
            }
        }

        public int GetUserId()
        {
            return (int)_contextAccessor.HttpContext.Session.GetInt32("userID");
        }
    }
}