using Demo.Entities;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MyBookings()
        {
            int userId = 1;

            var userBookings = await _context.UserBookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Event) 
                .Include(b => b.Payment) 
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            var bookingViewModels = userBookings.Select(b => new BookingViewModel
            {
                BookingId = b.Id,
                BookingDate = b.BookingDate,
                EventName = b.Event != null ? b.Event.EventName : "Unknown", 
                SeatsBooked = b.SeatsBooked,
                TotalAmount = b.Payment != null ? b.Payment.Amount : 0, 
                BookingStatus = b.BookingStatus
            }).ToList();

            return View(bookingViewModels);
        }

    }
}
