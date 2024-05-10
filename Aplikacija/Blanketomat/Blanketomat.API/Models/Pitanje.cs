using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Pitanje
{
    [Key]
    public int Id { get; set; }
    public required string Tekst { get; set; }
    public string? Slika { get; set; }
    public Oblast? Oblast { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}