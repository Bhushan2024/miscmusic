using Demo.Entities;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class PaymentViewModel
    {
        public int BookingId { get; set; }
        public decimal TotalAmount { get; set; }
        public int SeatsBooked { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal GstAmount { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 16 digits.")]
        [RegularExpression(@"^\d{13,16}$", ErrorMessage = "Card number must contain only digits.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card Holder Name is required")]
        public string CardHolderName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Card expiration date must be in the future.")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Range(001, 999, ErrorMessage = "CVV must be a 3-digit number.")]
        public int CVV { get; set; }
    }
}