using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class PonavljanjeIspitnogRoka
{
    [Key]
    public int Id { get; set; }

    [MaxLength(10)]
    public required string Datum { get; set; }
    public IspitniRok? IspitniRok { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}