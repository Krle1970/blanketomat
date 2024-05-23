using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Komentar
{
    [Key]
    public int Id { get; set; }

    [MinLength(5, ErrorMessage = "Tekst komentara mora imati minimum 5 karaktera")]
    [MaxLength(2000, ErrorMessage = "Ime administratora moze imati maksimalno 2000 karaktera")]
    public required string Tekst { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Broj lajkova mora biti pozitivan broj")]
    public int Lajkovi { get; set; }
    public Blanket? Blanket { get; set; }
    public List<Odgovor>? Odgovori { get; set; }
    public List<Slika>? Slika { get; set; }
    public Student? StudentPostavio { get; set; }
    public List<Profesor>? ProfesoriLiked { get; set; }
    public List<Asistent>? AsistentiLiked { get; set; }
}