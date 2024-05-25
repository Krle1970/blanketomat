using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.OblastDTOs;

public class OblastDTO
{
    [MinLength(3, ErrorMessage = "Naziv oblasti mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv oblasti moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public int? PredmetId { get; set; }
    public int[]? PodoblastiIds { get; set; }
    public int[]? PitanjaIds { get; set; }
    public int[]? ZadaciIds { get; set; }
}