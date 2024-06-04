using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.SmerDTOs;

public class SmerDTO
{
    [MinLength(3, ErrorMessage = "Naziv smera mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv smera moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public Katedra? Katedra { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
    public List<Student>? Studenti { get; set; }
}