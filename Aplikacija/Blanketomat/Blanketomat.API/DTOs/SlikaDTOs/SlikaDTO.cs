using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.SlikaDTOs;

public class SlikaDTO
{
    [MinLength(3, ErrorMessage = "Putanja mora imati minimalno 3 karaktera")]
    public required string Putanja { get; set; }

    // na kojim blanketima je slika
    public int[]? BlanketiIds { get; set; }

    // za koje pitanje je slika
    public int? PitanjeId { get; set; }

    // za koji zadatak je slika
    public int? ZadatakId { get; set; }

    // za koji komentar je slika
    public int? KomentarId { get; set; }

    // za koji odgovor je slika
    public int? OdgovorId { get; set; }
}