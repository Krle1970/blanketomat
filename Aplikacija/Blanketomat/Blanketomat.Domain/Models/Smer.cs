using System.ComponentModel.DataAnnotations;

namespace Blanketomat.Domain.Models;

public class Smer
{
    [Key]
    public int Id { get; set; }

    [MaxLength(40)]
    public required string Naziv { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
    public List<Student>? Studenti { get; set; }
}