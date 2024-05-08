using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Komentar
{
    [Key]
    public int Id { get; set; }
    public required string Tekst { get; set; }
    public int Lajkovi { get; set; }
    public List<Slika>? Slike { get; set; }
    public Student? StudentPostavio { get; set; }
    public List<Profesor>? ProfesoriLiked { get; set; }
    public List<Asistent>? AsistentiLiked { get; set; }
}