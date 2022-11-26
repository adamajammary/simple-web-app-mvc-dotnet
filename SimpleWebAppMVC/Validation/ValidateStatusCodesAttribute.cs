using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SimpleWebAppMVC.Validation
{
    public class ValidateStatusCodesAttribute : ValidationAttribute
    {
        public static readonly string[] ValidStatusCodes =
        {
            "N/A",
            "Not Started",
            "Started",
            "In Progress",
            "Almost Done",
            "Completed"
        };

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            return ValidStatusCodes.Contains(value?.ToString()) ? ValidationResult.Success : new ValidationResult("Invalid value");
        }
    }
}
