using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Administrator
{
    [Key]
    public int Id { get; set; }

    [MinLength(3, ErrorMessage = "Minimalna duzina imena je 3 karaktera")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina imena je 30 karaktera")]
    public required string Ime { get; set; }

    [MinLength(3, ErrorMessage = "Minimalna duzina prezimena je 3 karaktera")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina prezimena je 30 karaktera")]
    public required string Prezime { get; set; }

    [EmailAddress(ErrorMessage = "Nevalidna email adresa")]
    [MinLength(10, ErrorMessage = "Minimalna duzina email adrese je 10 karaktera")]
    [MaxLength(60, ErrorMessage = "Maksimalna duzina email adrese je 60 karaktera")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password hash je obavezno polje")]
    public required byte[] PasswordHash { get; set; }

    [Required(ErrorMessage = "Password salt je obavezno polje")]
    public required byte[] PasswordSalt { get; set; }
    public string? Token { get; set; }
}