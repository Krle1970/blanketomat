using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Akreditacija
{
    [Key]
    public int Id { get; set; }

    [MinLength(5, ErrorMessage = "Naziv akreditacije mora biti minimum 5 karaktera")]
    [MaxLength(40, ErrorMessage = "Naziv akreditacije moze biti maksimum 40 karaktera")]
    public required string Naziv { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Student>? Studenti { get; set; }
}