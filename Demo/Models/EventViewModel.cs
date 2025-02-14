using Demo.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class EventViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Event Name is required")]
        [MaxLength(200, ErrorMessage = "Event Name cannot exceed 200 characters")]
        public string EventName { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string EventDescription { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string EventLocation { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeSpan EventTime { get; set; }

        [Required(ErrorMessage = "Organizer name is required")]
        public string EventOrganizer { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ticket price must be greater than 0.")]
        public int TicketPrice { get; set; }


        [Required(ErrorMessage = "Organizer contact is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string EventOrganizerContact { get; set; }

        [Required(ErrorMessage = "Total seats are required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total seats must be at least 1")]
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }

        public bool IsBookingAvailable { get; set; }
    }
    public class UserEventViewModel
    {
        public List<Event> CurrentEvents { get; set; }
        public List<Event> UpcomingEvents { get; set; }
    }
}
