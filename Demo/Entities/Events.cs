using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string EventName { get; set; }

        [MaxLength(1000)]
        public string EventDescription { get; set; }

        [Required]
        [MaxLength(500)]
        public string EventLocation { get; set; }

        [Required]
        public DateTime EventDate { get; set; } 

        [Required]
        public TimeSpan EventTime { get; set; } 

        [Required]
        [MaxLength(200)]
        public string EventOrganizer { get; set; }

        [Required]
        [Phone] 
        public string EventOrganizerContact { get; set; }

        public bool IsBookingAvailable { get; set; } 

        [Required]
        [Range(1, int.MaxValue)] 
        public int TotalSeats { get; set; }

        [Range(0, int.MaxValue)] 
        public int BookedSeats { get; set; }
    }
}
