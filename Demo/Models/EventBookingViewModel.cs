using Demo.Entities;
using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public class EventBookingViewModel
    {
        public Event Event { get; set; }
        public List<UserBooking> Bookings { get; set; }
    }
}
