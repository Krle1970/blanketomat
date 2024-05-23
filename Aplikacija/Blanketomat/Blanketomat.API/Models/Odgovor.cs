using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Odgovor
{
    [Key]
    public int Id { get; set; }

    [MinLength(2, ErrorMessage = "Tekst odgovora mora imati minimum 2 karaktera")]
    [MaxLength(2000, ErrorMessage = "Tekst odgovora moze imati maksimalno 2000 karaktera")]
    public required string Tekst { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Broj lajkova mora biti pozitivan broj")]
    public required int Lajkovi { get; set; }

    // komentar na koji je odgovoreno
    public Komentar? Komentar { get; set; }
    public List<Slika>? Slike { get; set; }
    public Student? StudentPostavio { get; set; }
    public List<Profesor>? ProfesoriVerifikovali { get; set; }
    public List<Asistent>? AsistentiVerifikovali { get; set; }
}