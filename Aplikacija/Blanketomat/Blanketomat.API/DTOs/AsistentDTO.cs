using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs;

public class AsistentDTO
{
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

    [MinLength(5, ErrorMessage = "Minimalna duzina sifre je 5 karaktera")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina sifre je 30 karaktera")]
    public required string Password { get; set; }
    public int? KatedraId {  get; set; }
    public int[]? SmeroviIds { get; set; }
    public int[]? PredmetiIds { get; set; }
}