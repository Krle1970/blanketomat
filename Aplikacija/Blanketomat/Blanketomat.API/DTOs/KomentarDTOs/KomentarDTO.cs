using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.KomentarDTOs;

public class KomentarDTO
{
    [MinLength(5, ErrorMessage = "Tekst komentara mora imati minimum 5 karaktera")]
    [MaxLength(2000, ErrorMessage = "Tekst komentara moze imati maksimalno 2000 karaktera")]
    public required string Tekst { get; set; }
    public Blanket? Blanket { get; set; }
    public List<Slika>? Slike { get; set; }
    public Student? StudentPostavio { get; set; }
}