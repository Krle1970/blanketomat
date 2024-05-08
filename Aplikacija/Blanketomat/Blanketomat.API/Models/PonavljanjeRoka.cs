using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class PonavljanjeRoka
{
    [Key]
    public int Id { get; set; }
    public required string Datum { get; set; }
    public IspitniRok? IspitniRok { get; set; }
<<<<<<< HEAD
    public List<Blanket>? Blanketi { get; set; }
=======

    public List<Blanket>? Blanketi{ get; set; }
>>>>>>> ddc0c7b6a56afeb10a1c0bc8e41f3696fff4a262
}