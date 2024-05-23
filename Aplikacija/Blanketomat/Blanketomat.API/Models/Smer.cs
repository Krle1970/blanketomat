using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Smer
{
    [Key]
    public int Id { get; set; }

    [MinLength(3, ErrorMessage = "Naziv smera mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv smera moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public Katedra? Katedra { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
    public List<Student>? Studenti { get; set; }
}