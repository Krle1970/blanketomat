using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class PonavljanjeRoka
{
    [Key]
    public int Id { get; set; }
    public required string Datum { get; set; }
    public IspitniRok? IspitniRok { get; set; }

    public List<Blanket>? Blanketi{ get; set; }
}