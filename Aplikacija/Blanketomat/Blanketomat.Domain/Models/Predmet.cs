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
    public Akreditacija? Akreditacija { get; set; }
    public Smer? Smer { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
    public List<Student>? Studenti { get; set; }
}