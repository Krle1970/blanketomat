using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.PodoblastDTOs;

public class PodoblastDTO
{
    [MinLength(3, ErrorMessage = "Naziv podoblasti mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv podoblasti moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public Oblast? Oblast { get; set; }
    public List<Zadatak>? Zadaci { get; set; }
}