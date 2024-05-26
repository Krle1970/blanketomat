using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.ZadatakDTOs;

public class ZadatakDTO
{
    [MinLength(5, ErrorMessage = "Tekst zadatka mora imati minimum 5 karaktera")]
    [MaxLength(1000, ErrorMessage = "Tekst zadatka moze imati maksimalno 1000 karaktera")]
    public required string Tekst { get; set; }
    public int[]? SlikeIds { get; set; }
    public int? OblastId { get; set; }
    public int[]? PodoblastiIds { get; set; }
    public int[]? BlanketiIds { get; set; }
}