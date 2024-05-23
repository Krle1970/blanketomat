using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Katedra
{
    [Key]
    public int Id { get; set; }

    [MinLength(5, ErrorMessage = "Naziv katedre mora imati minimum 5 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv katedre moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public List<Smer>? Smerovi { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
}