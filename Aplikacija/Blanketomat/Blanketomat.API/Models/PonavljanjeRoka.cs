using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class PonavljanjeRoka
{
    [Key]
    public int Id { get; set; }
    public required DateOnly Datum { get; set; }
    public IspitniRok? IspitniRok { get; set; }
}