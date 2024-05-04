using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Oblast
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public required string Naziv { get; set; }
    public List<Pitanje>? Pitanja { get; set; }
    public List<Zadatak>? Zadaci { get; set; }
    public List<Blanket>? Blanketi { get; set; }
}