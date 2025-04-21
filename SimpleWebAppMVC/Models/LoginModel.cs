using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebAppMVC.Models
{
    public class LoginModel : AuthModel
    {
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        [FromQuery]
        public string ReturnUrl { get; set; }
    }
}
