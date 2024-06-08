using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.OblastDTOs;

public class OblastDTO
{   public int Id { get; set; }
    [MinLength(3, ErrorMessage = "Naziv oblasti mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv oblasti moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }
    public Predmet? Predmet { get; set; }
    public List<Podoblast>? Podoblasti { get; set; }
    public List<Pitanje>? Pitanja { get; set; }
    public List<Zadatak>? Zadaci { get; set; }
}