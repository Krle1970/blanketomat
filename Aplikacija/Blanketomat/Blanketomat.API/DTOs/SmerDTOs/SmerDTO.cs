using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.SmerDTOs;

public class SmerDTO
{
    [MinLength(3, ErrorMessage = "Naziv smera mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv smera moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public int? KatedraId { get; set; }
    public int[]? PredmetiIds { get; set; }
    public int[]? ProfesoriIds { get; set; }
    public int[]? AsistentiIds { get; set; }
    public int[]? StudentiIds { get; set; }
}