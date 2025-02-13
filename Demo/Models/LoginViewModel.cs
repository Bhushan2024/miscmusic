using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Demo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Email Required")]
        [DisplayName("Username or Email")]
        public string UsernameOrEmail { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
}
