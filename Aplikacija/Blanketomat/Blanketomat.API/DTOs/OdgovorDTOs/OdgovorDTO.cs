using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.OdgovorDTOs;

public class OdgovorDTO
{
    [MinLength(5, ErrorMessage = "Tekst odgovora mora imati minimum 2 karaktera")]
    [MaxLength(2000, ErrorMessage = "Tekst odgovora moze imati maksimalno 2000 karaktera")]
    public required string Tekst { get; set; }
    public int? KomentarId { get; set; }
    public int[]? SlikeIds { get; set; }
    public int? StudentPostavioId { get; set; }
}