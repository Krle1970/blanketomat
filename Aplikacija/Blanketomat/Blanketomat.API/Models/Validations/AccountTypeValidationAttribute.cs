using Blanketomat.API.DTOs.LoginDTOs;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models.Validations
{
    public class AccountTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var user = validationContext.ObjectInstance as LoginRequestDTO;

            if (user != null)
            {
                if (user.AccountType != "Administrator" && user.AccountType != "Profesor" && user.AccountType != "Asistent" && 
                    user.AccountType != "Student")
                {
                    return new ValidationResult("Tip naloga mora biti Administrator, Profesor, Asistent ili Student");
                }
            }

            return ValidationResult.Success;
        }
    }
}
