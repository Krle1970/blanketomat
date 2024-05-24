using Blanketomat.API.DTOs.BlanketDTOs;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models.Validations;

public class TipBlanketaValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var blanket = validationContext.ObjectInstance as BlanketDTO;

        if (blanket != null)
        {
            if (blanket.Tip != "Pismeni" && blanket.Tip != "Usmeni" && blanket.Tip != "Pismeni/Usmeni")
            {
                return new ValidationResult("Tip blanketa mora biti Pismeni, Usmeni ili Pismeni/Usmeni");
            }
        }

        return ValidationResult.Success;
    }
}