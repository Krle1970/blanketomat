using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.AkreditacijaDTOs;

public class AkreditacijaDTO
{
    [MinLength(5, ErrorMessage = "Naziv akreditacije mora biti minimum 5 karaktera")]
    [MaxLength(40, ErrorMessage = "Naziv akreditacije moze biti maksimum 40 karaktera")]
    public required string Naziv { get; set; }
    public List<Predmet>? Predmeti { get; set; }
    public List<Student>? Studenti { get; set; }
}