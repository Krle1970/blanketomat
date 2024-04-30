using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Akreditacija
{
    [Key]
    public int Id { get; set; }

    [MaxLength(40)]
    public required string Naziv { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Student>? Studenti { get; set; }
}