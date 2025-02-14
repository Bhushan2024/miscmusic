using System;
using System.ComponentModel.DataAnnotations;
namespace Demo.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Adminemail { get; set; }
        
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Adminpassword { get; set; }
    }
}
