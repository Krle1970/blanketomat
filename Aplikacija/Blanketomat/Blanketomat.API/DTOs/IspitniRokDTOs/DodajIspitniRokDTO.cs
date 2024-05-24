using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.IspitniRokDTOs;

public class DodajIspitniRokDTO
{
    [MinLength(3, ErrorMessage = "Naziv ispitnog roka mora imati minimum 3 karaktera")]
    [MaxLength(30, ErrorMessage = "Naziv ispitnog roka moze imati maksimalno 30 karaktera")]
    public required string Naziv { get; set; }
}