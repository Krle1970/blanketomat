using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Pitanje
{
    [Key]
    public int Id { get; set; }

    [MinLength(5, ErrorMessage = "Tekst pitanja mora imati minimum 5 karaktera")]
    [MaxLength(500, ErrorMessage = "Tekst pitanja moze imati maksimalno 500 karaktera")]
    public required string Tekst { get; set; }
    public List<Slika>? Slika { get; set; }
    public Oblast? Oblast { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}