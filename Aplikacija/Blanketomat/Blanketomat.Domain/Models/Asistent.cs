using System.ComponentModel.DataAnnotations;

namespace Blanketomat.Domain.Models;

public class Asistent
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public required string Ime { get; set; }

    [MaxLength(20)]
    public required string Prezime { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    public required byte[] Password { get; set; }
    public Smer? Smer { get; set; }
    public List<Predmet>? Predmeti { get; set; }
}