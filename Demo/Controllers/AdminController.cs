using Demo.Entities;
using Demo.Migrations;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    TicketPrice = model.TicketPrice,
                    TotalSeats = model.TotalSeats,
                    BookedSeats = 0
                };

                _context.Events.Add(newEvent);
                _context.SaveChanges();

                return RedirectToAction("AdminHomePage", "Account");
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ToggleBookingStatus(int eventId)
        {
            var eventEntity = _context.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            if (eventEntity.EventDate > DateTime.Now && eventEntity.BookedSeats < eventEntity.TotalSeats)
            {
                eventEntity.IsBookingAvailable = !eventEntity.IsBookingAvailable;
                _context.SaveChanges();
                return RedirectToAction("AdminHomePage", "Account");
            }

            return BadRequest(new { success = false, message = "Cannot enable booking. Event is either in the past or fully booked." });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminBooking(int eventId)
        {
            var eventDetails = _context.Events
                .Where(e => e.Id == eventId)
                .FirstOrDefault();

            if (eventDetails == null)
            {
                return NotFound();
            }

            var bookings = _context.UserBookings
                .Where(b => b.EventId == eventId)
                .Include(b => b.User) 
                .ToList();


            var viewModel = new EventBookingViewModel
            {
                Event = eventDetails,
                Bookings = bookings
            };

            return View(viewModel);
        }
    }
}

