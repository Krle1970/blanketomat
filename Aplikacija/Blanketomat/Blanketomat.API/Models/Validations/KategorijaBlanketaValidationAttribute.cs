using Blanketomat.API.DTOs.BlanketDTOs;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models.Validations;

public class KategorijaBlanketaValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var blanket = validationContext.ObjectInstance as BlanketDTO;

        if (blanket != null)
        {
            if (blanket.Kategorija != "I kolokvijum" && blanket.Kategorija != "II kolokvijum" &&
                blanket.Kategorija != "III kolokvijum" && blanket.Kategorija != "IV kolokvijum" &&
                blanket.Kategorija != "I kolokvijum popravni" && blanket.Kategorija != "II kolokvijum popravni" &&
                blanket.Kategorija != "III kolokvijum popravni" && blanket.Kategorija != "IV kolokvijum popravni" &&
                blanket.Kategorija != "Ispit" && blanket.Kategorija != "Ispit popravni")
            {
                return new ValidationResult("Kategorija blanketa mora biti I, II, III, IV kolokvijum ili Ispit");
            }
        }

        return ValidationResult.Success;
    }
}