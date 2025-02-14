namespace Demo.Models
{
    public class UserBookingViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public int SeatsBooked { get; set; }
    }
}
