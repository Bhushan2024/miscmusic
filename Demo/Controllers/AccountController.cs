using Demo.Entities;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Static Admin Credentials
        private const string AdminEmail = "admin@example.com";
        private const string AdminPassword = "Admin@123";

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_context.Events.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserAccount account = new UserAccount
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password
                };

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.Firstname} {account.Lastname} registered successfully, You can login now";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
                return View();
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user is Admin
                if (model.UsernameOrEmail == AdminEmail && model.Password == AdminPassword)
                {
                    var adminClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Admin"),
                        new Claim("Name", "Admin"),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(adminIdentity));

                    return RedirectToAction("AdminHomePage");
                }

                // Check if the user is a regular user from the database
                var user = _context.UserAccounts
                    .Where(u => (u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail) && u.Password == model.Password)
                    .FirstOrDefault();

                if (user != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Firstname),
                        new Claim("Name", user.Firstname),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "User")
                    };

                    var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

                    return RedirectToAction("UserHomePage", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username/Email or Password");
                }
            }
            return View(model);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); 
        }

        

        [Authorize(Roles = "Admin")]
        public IActionResult AdminHomePage()
        {
            var events = _context.Events
                .Select(e => new EventViewModel
                {
                    Id=e.Id,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventLocation = e.EventLocation,
                    EventDate = e.EventDate,
                    EventTime = e.EventTime,
                    EventOrganizer = e.EventOrganizer,
                    EventOrganizerContact = e.EventOrganizerContact,
                    IsBookingAvailable = e.IsBookingAvailable,
                    TotalSeats = e.TotalSeats,
                    BookedSeats = e.BookedSeats
                }).ToList();

            // 🔹 Ensure the model is not null before passing it to the view
            return View(events ?? new List<EventViewModel>());
        }

    }
}
