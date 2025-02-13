using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Demo.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="First Name Required")]
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

    }
}
    