using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.PonavljanjeIspitnogRokaDTOs;

public class PonavljanjeIspitnogRokaDTO
{
    [MinLength(3, ErrorMessage = "Naziv ispitnog roka mora imati minimum 3 karaktera")]
    [MaxLength(30, ErrorMessage = "Naziv ispitnog roka moze imati maksimalno 30 karaktera")]
    public required string Naziv { get; set; }

    [Required(ErrorMessage = "Datum odrzavanja ispita je obavezno polje")]
    public required DateOnly Datum { get; set; }
    public IspitniRok? IspitniRok { get; set; }
}