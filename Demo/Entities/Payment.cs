using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 16 digits.")]
        [RegularExpression(@"^\d{13,16}$", ErrorMessage = "Card number must contain only digits.")]
        public string CardNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Card expiration date must be in the future.")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Range(001, 999, ErrorMessage = "CVV must be a 3-digit number.")]
        public int CVV { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Cardholder name can't be longer than 100 characters.")]
        public string CardHolderName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Transaction ID can't be longer than 50 characters.")]
        public string TransactionId { get; set; }

        // Status could be "Success", "Failed", etc.
        [Required]
        [MaxLength(50)]
        [RegularExpression("Success|Failed", ErrorMessage = "Transaction status must be either Success or Failed.")]
        public string Status { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Now;
            }
            return false;
        }
    }
}
