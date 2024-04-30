using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Asistent
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public required string Ime { get; set; }

    [MaxLength(20)]
    public required string Prezime { get; set; }

    [MaxLength(50)]
    [EmailAddress]
    public required string Email { get; set; }

    [MaxLength(30)]
    public required string Password { get; set; }
    public Smer? Smer { get; set; }
    public List<Predmet>? Predmeti { get; set; }
}