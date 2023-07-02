﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using Restauracja.ViewModels;
using Restauracja.Services;

namespace Restauracja.Controllers
{
    public class UsersController : Controller
    {
        private readonly RestauracjaContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;

        public UsersController(RestauracjaContext context, IHttpContextAccessor contextAccessor, IUserService userService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index(int page = 1)
        {
            if (_context.Dish != null)
            {
                PaginationViewModel<User> viewModel = await _userService.FillPaginationViewModelAsync(page);
                return View(viewModel);
            }
            else
            {
                return Problem("Entity set 'RestauracjaContext.Dish'  is null.");
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,City,PostalCode,Address,PhoneNumber,Password")] UserViewModel userVM)
        {
            var passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(null, userVM.Password);
            User user = new User()
            {
                Email = userVM.Email,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                City = userVM.City,
                PostalCode = userVM.PostalCode,
                Address = userVM.Address,
                PhoneNumber = userVM.PhoneNumber,
                PasswordHash = hashedPassword,
                ActivationCode = Guid.NewGuid().ToString()
            };
            user.Role = _context.Role.FindAsync(1).Result;

            bool emailExists = _context.User.ToList().Where(u => u.Email == user.Email).Any();
            bool phoneNumberExists = _context.User.ToList().Where(u => u.PhoneNumber == user.PhoneNumber).Any();

            if (emailExists)
            {
                ModelState.AddModelError("", "Email jest zajęty");
            }

            if (phoneNumberExists)
            {
                ModelState.AddModelError("", "Numer telefonu jest zajęty");
            }

            foreach (var item in ModelState.Values)
            {
                foreach (var item2 in item.Errors)
                {
                    Console.WriteLine(item2.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("restauracjarestauracja658@gmail.com", "nawxtadxbtwwuxwa"),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("restauracjarestauracja658@gmail.com"),
                    Subject = "Link aktywacyjny",
                    Body = "<h1>Witaj oto twój link aktywacyjny: <br/>" +
                    "</h1>" +
                    "<a href='https://localhost:7015/Users/Activation?activationCode=" + user.ActivationCode + "'>Aktywuj konto</a>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(user.Email);

                smtpClient.Send(mailMessage);
                return RedirectToAction("RegisterSuccess", "Users");
            }
            return View(userVM);
        }

        public IActionResult Activation(string activationCode)
        {
            var user = _context.User.ToList().Find(u => u.ActivationCode == activationCode);
            if (user != null)
            {
                user.IsActive = true;
                _context.Update(user);
                _context.SaveChanges();
            }
            return View();
        }
        public IActionResult RegisterSuccess()
        {
            return View();
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,FirstName,LastName,City,PostalCode,Address,PhoneNumber,PasswordHash")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'RestauracjaContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel loginVM)
        {
            User user = await _context.User
                .Include(r => r.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == loginVM.Email);

            if (user != null)
            {
                if (user.IsActive)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginVM.Password);
                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        _contextAccessor.HttpContext.Session.SetString("role", user.Role.Name);
                        _contextAccessor.HttpContext.Session.SetString("firstName", user.FirstName);
                        _contextAccessor.HttpContext.Session.SetString("lastName", user.LastName);
                        _contextAccessor.HttpContext.Session.SetInt32("userID", user.UserId);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Hasło jest błędne");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Konto nie zostało aktywowane");
                }


            }
            else
            {
                ModelState.AddModelError("", "Konto o podanym mailu nie istnieje");
            }

            return View(loginVM);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("firstName");
            HttpContext.Session.Remove("lastName");
            HttpContext.Session.Remove("userID");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //public IActionResult ActivateOrDeactivateUser(int userID)
        //{
        //    Console.WriteLine(userID);
        //    return RedirectToAction("Index", "Users");
        //}
    }
}
