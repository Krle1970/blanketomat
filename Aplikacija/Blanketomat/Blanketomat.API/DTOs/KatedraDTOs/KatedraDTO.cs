using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.KatedraDTOs;

public class KatedraDTO
{
    [MinLength(5, ErrorMessage = "Naziv katedre mora imati minimum 5 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv katedre moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public int[]? Smerovi { get; set; }
    public int[]? Profesori { get; set; }
    public int[]? Asistenti { get; set; }
}