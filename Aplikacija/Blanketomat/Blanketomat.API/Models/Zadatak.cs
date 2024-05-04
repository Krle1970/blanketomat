using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Zadatak
{
    [Key]
    public int Id { get; set; }
    public required string Tekst { get; set; }
    public Slika? Slika { get; set; }
    public Oblast? Oblast { get; set; }
    public Podoblast? Podoblast { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}