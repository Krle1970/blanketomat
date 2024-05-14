using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Katedra
{
    [Key]
    public int Id { get; set; }

    [MaxLength(40)]
    public required string Naziv { get; set; }
    public List<Smer>? Smerovi { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
}