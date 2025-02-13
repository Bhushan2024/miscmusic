using Demo.Entities;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Demo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Show Admin Home Page with List of Events
        public IActionResult AdminHomePage()
        {
            var events = _context.Events
                .Select(e => new EventViewModel
                {
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

            return View(events);
        }

        // Show the Add Event Form
        public IActionResult AddEvent()
        {
            return View(new EventViewModel());
        }

        [HttpPost]
        public IActionResult AddEvent(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                Event newEvent = new Event
                {
                    EventName = model.EventName,
                    EventDescription = model.EventDescription,
                    EventLocation = model.EventLocation,
                    EventDate = model.EventDate,
                    EventTime = model.EventTime,
                    EventOrganizer = model.EventOrganizer,
                    EventOrganizerContact = model.EventOrganizerContact,
                    IsBookingAvailable = model.IsBookingAvailable,
                    TotalSeats = model.TotalSeats,
                    BookedSeats = 0
                };

                _context.Events.Add(newEvent);
                _context.SaveChanges();

                return RedirectToAction("AdminHomePage", "Account");
            }

            return View(model);
        }

    }
}
