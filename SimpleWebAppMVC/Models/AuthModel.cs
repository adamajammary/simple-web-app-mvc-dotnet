using System.ComponentModel.DataAnnotations;

namespace SimpleWebAppMVC.Models
{
    public class AuthModel
    {
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }
    }
}
