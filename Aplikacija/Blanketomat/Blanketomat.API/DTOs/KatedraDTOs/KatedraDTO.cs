using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.KatedraDTOs;

public class KatedraDTO
{
    [MinLength(5, ErrorMessage = "Naziv katedre mora imati minimum 5 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv katedre moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public List<Smer>? Smerovi { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
}