using System.ComponentModel.DataAnnotations;

namespace Blanketomat.Domain.Models;

public class Predmet
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public required string Naziv { get; set; }

    [MaxLength(4)]
    public required string Godina { get; set; }
    public required Akreditacija Akreditacija { get; set; }
    public required Smer Smer { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
    public List<Student>? Studenti { get; set; }
}