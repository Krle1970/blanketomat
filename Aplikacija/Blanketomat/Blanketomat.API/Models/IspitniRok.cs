using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class IspitniRok
{
    [Key]
    public int Id { get; set; }

    [MinLength(3, ErrorMessage = "Naziv ispitnog roka mora imati minimum 3 karaktera")]
    [MaxLength(30, ErrorMessage = "Naziv ispitnog roka moze imati maksimalno 30 karaktera")]
    public required string Naziv { get; set; }
    public List<PonavljanjeIspitnogRoka>? Ponavljanja { get; set; }
}