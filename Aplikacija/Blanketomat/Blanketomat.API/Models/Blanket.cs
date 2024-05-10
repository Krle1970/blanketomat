using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Blanket
{
    [Key]
    public int Id { get; set; }

    // Tip - pismeni, usmeni, oba
    [MaxLength(30)]
    public required string Tip { get; set; }

    // Kategorija - I kolok, II kolok, Ispit...
    [MaxLength(30)]
    public required string Kategorija { get; set; }
    public PonavljanjeIspitnogRoka? IspitniRok { get; set; }
    public List<Pitanje>? Pitanja { get; set; }
    public List<Zadatak>? Zadaci { get; set; }
}