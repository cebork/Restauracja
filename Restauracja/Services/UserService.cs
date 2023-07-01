using Restauracja.Data;
using System;

namespace Restauracja.Services
{
    public interface IUserService
    {
        bool checkIfSessionIsSet();
        bool CheckIfLoggedIn();
        bool CheckIfAdmin();
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

        
    }
}
