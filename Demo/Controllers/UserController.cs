using Demo.Entities;
using Demo.Models;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Demo.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        public IActionResult UserHomePage()
        {
            // Get current events (booking open)
            var currentEvents = _context.Events
                                        .Where(e => e.IsBookingAvailable && e.EventDate >= DateTime.Now)
                                        .ToList();

            // Get upcoming events (booking closed)
            var upcomingEvents = _context.Events
                                         .Where(e => e.EventDate > DateTime.Now && !e.IsBookingAvailable)
                                         .ToList();

            var viewModel = new UserEventViewModel
            {
                CurrentEvents = currentEvents,
                UpcomingEvents = upcomingEvents
            };

            return View(viewModel);
        }

        [Authorize(Roles = "User")]
        public IActionResult BookEvent(int eventId)
        {
            var eventDetails = _context.Events
                .Where(e => e.Id == eventId)
                .FirstOrDefault();

            if (eventDetails == null)
            {
                return NotFound();
            }

            var model = new UserBookingViewModel
            {
                EventId = eventDetails.Id,
                EventName = eventDetails.EventName,
                EventDescription = eventDetails.EventDescription,
                EventLocation = eventDetails.EventLocation,
                EventDate = eventDetails.EventDate,
                EventTime = eventDetails.EventTime,
                TotalSeats = eventDetails.TotalSeats,
                BookedSeats = eventDetails.BookedSeats
            };

            return View(model);
        }

        // Action to process booking form submission
        [HttpPost]
        public IActionResult BookEvent(UserBookingViewModel model)
        {
            var eventDetails = _context.Events
                .Where(e => e.Id == model.EventId)
                .FirstOrDefault();

            if (eventDetails == null)
            {
                return NotFound();
            }

            // Check if the selected seats are available
            if (model.SeatsBooked > eventDetails.TotalSeats - eventDetails.BookedSeats)
            {
                ModelState.AddModelError("SeatsBooked", "Not enough seats available.");
                return View(model); 
            }
            string stringUserId = User.FindFirstValue("UserId");  
            int userId = int.Parse(stringUserId);
            //var userId = 1; 
            var userBooking = new UserBooking
            {
                EventId = model.EventId,
                UserId = userId,
                SeatsBooked = model.SeatsBooked,
                BookingStatus = "Pending",
                BookingDate = DateTime.Now
            };

            _context.UserBookings.Add(userBooking);
            _context.SaveChanges();

            var ticketPrice = eventDetails.TicketPrice; 
            var gstRate = 0.18m; 
            var gstAmount = ticketPrice * model.SeatsBooked * gstRate;
            var totalAmount = (ticketPrice * model.SeatsBooked) + gstAmount;

            return RedirectToAction("Payment", new
            {
                bookingId = userBooking.Id,
                totalAmount,
                seatsBooked = model.SeatsBooked,
                ticketPrice,
                gstAmount
            });
        }


        // Action to show payment page
        [Authorize(Roles = "User")]
        public IActionResult Payment(int bookingId, decimal totalAmount, int seatsBooked, decimal ticketPrice, decimal gstAmount)
        {
            var model = new PaymentViewModel
            {
                BookingId = bookingId,
                TotalAmount = totalAmount,
                SeatsBooked = seatsBooked,
                TicketPrice = ticketPrice,
                GstAmount = gstAmount 
            };

            return View(model);
        }


        // Action to process payment and confirm booking
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Payment(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process payment here (This is a placeholder for actual payment logic)
                var paymentDetails = new Payment
                {
                    Amount = model.TotalAmount,
                    TransactionDate = DateTime.Now,
                    CardNumber = model.CardNumber,
                    CardHolderName = model.CardHolderName,
                    ExpiryDate = model.ExpiryDate,
                    CVV = model.CVV,
                    Status = "Success",
                    TransactionId = Guid.NewGuid().ToString() 
                };

                _context.Payments.Add(paymentDetails);
                _context.SaveChanges(); // Save payment details to the database

                // Get the booking information from the database
                var booking = _context.UserBookings.Find(model.BookingId);
                if (booking != null)
                {
                    // Update booking status to 'Confirmed' and link payment to booking
                    booking.BookingStatus = "Confirmed";
                    booking.PaymentId = paymentDetails.Id; // Link Payment ID to UserBooking
                    _context.SaveChanges();

                    // Increase booked seats in the User table for the specific event
                    var eventDetails = _context.Events.Find(booking.EventId);
                    if (eventDetails != null)
                    {
                        eventDetails.BookedSeats += booking.SeatsBooked; // Update booked seats count
                        _context.SaveChanges();
                    }
                }

                // Redirect to the home page with success message
                TempData["SuccessMessage"] = "Booking confirmed! Congratulations.";
                return RedirectToAction("UserHomePage");
            }

            // Return to payment page if there are validation errors
            return View("Payment", model);
        }


        // Action for booking confirmation
        public IActionResult BookingConfirmation(int bookingId)
        {
            var booking = _context.UserBookings
                .Where(b => b.Id == bookingId)
                .FirstOrDefault();

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
