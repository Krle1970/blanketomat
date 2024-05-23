using Blanketomat.API.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Blanket
{
    [Key]
    public int Id { get; set; }

    // Tip - pismeni, usmeni, oba
    [MinLength(3, ErrorMessage = "Tip blanketa mora imati minimalno 3 karaktera")]
    [MaxLength(20, ErrorMessage = "Tip blanketa moze imati maksimalno 20 karaktera")]
    [TipBlanketaValidation]
    public required string Tip { get; set; }

    // Kategorija - I kolok, II kolok, Ispit...
    [MinLength(5, ErrorMessage = "Kategorija blanketa mora imati minimalno 5 karaktera")]
    [MaxLength(50, ErrorMessage = "Kategorija blanketa moze imati maksimalno 50 karaktera")]
    [KategorijaBlanketaValidation]
    public required string Kategorija { get; set; }

    [MinLength(3, ErrorMessage = "Putanja mora imati minimalno 3 karaktera")]
    public required string Putanja { get; set; }
    public List<Slika>? Slike { get; set; }
    public Predmet? Predmet { get; set; }
    public PonavljanjeIspitnogRoka? IspitniRok { get; set; }
    public List<Pitanje>? Pitanja { get; set; }
    public List<Zadatak>? Zadaci { get; set; }
    public List<Komentar>? Komentari { get; set; }
}