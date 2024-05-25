using Blanketomat.API.DTOs.PredmetDTOs;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models.Validations;

public class PredmetGodinaValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var predmet = validationContext.ObjectInstance as PredmetDTO;

        if (predmet != null)
        {
            if (predmet.Godina != "I" && predmet.Godina != "II" &&
                predmet.Godina != "III" && predmet.Godina != "IV")
            {
                return new ValidationResult("Godina na kojoj je predmet mora biti I, II, III ili IV");
            }
        }

        return ValidationResult.Success;
    }
}