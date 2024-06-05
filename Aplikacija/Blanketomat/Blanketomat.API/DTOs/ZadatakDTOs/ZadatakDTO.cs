using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.ZadatakDTOs;

public class ZadatakDTO
{
    [MinLength(5, ErrorMessage = "Tekst zadatka mora imati minimum 5 karaktera")]
    [MaxLength(1000, ErrorMessage = "Tekst zadatka moze imati maksimalno 1000 karaktera")]
    public required string Tekst { get; set; }
    public List<Slika>? Slike { get; set; }
    public Oblast? Oblast { get; set; }
    public List<Podoblast>? Podoblasti { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}