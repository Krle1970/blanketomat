using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Slika
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string PutanjaDoSlike { get; set; }
    public List<Pitanje>? Pitanja { get; set; }
    public List<Zadatak>? Zadaci { get; set; }
    public Komentar? Komentar { get; set; }
}