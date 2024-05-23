using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Slika
{
    public int Id { get; set; }

    [MinLength(3, ErrorMessage = "Putanja mora imati minimalno 3 karaktera")]
    public required string Putanja { get; set; }

    // na kojim blanketima je slika
    public List<Blanket>? Blanketi { get; set; }

    // za koje pitanje je slika
    public Pitanje? Pitanje { get; set; }

    // za koji zadatak je slika
    public Zadatak? Zadatak { get; set; }

    // za koji komentar je slika
    public Komentar? Komentar { get; set; }

    // za koji odgovor je slika
    public Odgovor? Odgovor { get; set; }
}