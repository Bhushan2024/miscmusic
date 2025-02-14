using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Entities
{
    public class UserBooking
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserAccount User { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You must book at least one seat.")]
        public int SeatsBooked { get; set; }

        // Booking status can be "Pending" or "Confirmed"
        [Required]
        [MaxLength(50)]
        [RegularExpression("Pending|Confirmed", ErrorMessage = "Booking Status must be either Pending or Confirmed.")]
        public string BookingStatus { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        // Make PaymentId nullable
        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
    }

}
