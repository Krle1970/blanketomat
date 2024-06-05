using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.OdgovorDTOs;

public class OdgovorDTO
{
    [MinLength(5, ErrorMessage = "Tekst odgovora mora imati minimum 2 karaktera")]
    [MaxLength(2000, ErrorMessage = "Tekst odgovora moze imati maksimalno 2000 karaktera")]
    public required string Tekst { get; set; }
    public Komentar? Komentar { get; set; }
    public List<Slika>? Slike { get; set; }
    public Student? StudentPostavio { get; set; }
}