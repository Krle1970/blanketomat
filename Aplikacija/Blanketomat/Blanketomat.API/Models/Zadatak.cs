using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Zadatak
{
    [Key]
    public int Id { get; set; }

    [MinLength(5, ErrorMessage = "Tekst zadatka mora imati minimum 5 karaktera")]
    [MaxLength(1000, ErrorMessage = "Tekst zadatka moze imati maksimalno 1000 karaktera")]
    public required string Tekst { get; set; }
    public List<Slika>? Slika { get; set; }
    public Oblast? Oblast { get; set; }
    public List<Podoblast>? Podoblast { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}