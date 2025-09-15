using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using JanHofmeyerAdmin.Data;
using Microsoft.EntityFrameworkCore;

namespace JanHofmeyerAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // Otherwise show login page
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                // Find user
                var admin = await _context.AdminUsers
                    .FirstOrDefaultAsync(a => a.Username == username);

                if (admin == null)
                {
                    ViewBag.Error = "Invalid username or password";
                    return View();
                }

                // Verify password
                bool isValid = BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);

                if (!isValid)
                {
                    ViewBag.Error = "Invalid username or password";
                    return View();
                }

                // Create login session
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString())
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred. Please try again.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // TEMPORARY: Remove this after fixing login
        public IActionResult CreatePassword()
        {
            string newPassword = "Admin123!";
            string hash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            ViewBag.Password = newPassword;
            ViewBag.Hash = hash;

            return View();
        }


    }
}