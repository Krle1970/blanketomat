using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class IspitniRok
{
    [Key]
    public int Id { get; set; }

    [MaxLength(40)]
    public required string Naziv { get; set; }
    public List<PonavljanjeRoka>? Ponavljanja { get; set; }
}