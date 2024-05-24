using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models.Validations
{
    public class AccountTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var tipNaloga = validationContext.ObjectInstance as string;

            if (tipNaloga != null)
            {
                if (tipNaloga != "Administrator" || tipNaloga != "Profesor" || tipNaloga != "Asistent" || 
                    tipNaloga != "Student")
                {
                    return new ValidationResult("Tip naloga mora biti Administrator, Profesor, Asistent ili Student");
                }
            }

            return ValidationResult.Success;
        }
    }
}
