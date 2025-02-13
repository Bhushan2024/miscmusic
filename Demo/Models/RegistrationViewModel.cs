using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First Name Required")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        [DataType(DataType.Password)]
        public string confimPassword { get; set; }
    }
}
