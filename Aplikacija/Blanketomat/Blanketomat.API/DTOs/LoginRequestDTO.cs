using Blanketomat.API.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs;

public class LoginRequestDTO
{
    [EmailAddress(ErrorMessage = "Nevalidna email adresa")]
    [MinLength(10, ErrorMessage = "Minimalna duzina email adrese je 10 karaktera")]
    [MaxLength(60, ErrorMessage = "Maksimalna duzina email adrese je 60 karaktera")]
    public required string Email { get; set; }

    [MinLength(5, ErrorMessage = "Minimalna duzina sifre je 5 karaktera")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina sifre je 30 karaktera")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Tip naloga je obavezno polje")]
    [TipBlanketaValidation]
    public required string AccountType { get; set; }
}