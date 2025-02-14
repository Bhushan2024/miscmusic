using System;

namespace Demo.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public string EventName { get; set; }
        public int SeatsBooked { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; } 
    }
}
