using Blanketomat.API.Models;
using Blanketomat.API.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.PredmetDTOs;

public class PredmetDTO
{
    [MinLength(3, ErrorMessage = "Naziv predmeta mora imati minimum 3 karaktera")]
    [MaxLength(50, ErrorMessage = "Naziv predmeta moze imati maksimalno 50 karaktera")]
    public required string Naziv { get; set; }

    [MinLength(1, ErrorMessage = "Godina na kojoj je predmet mora imati minimum 1 karaktera")]
    [MaxLength(4, ErrorMessage = "Godina na kojoj je predmet moze imati maksimalno 4 karaktera")]
    [PredmetGodinaValidation]
    public required string Godina { get; set; }
    public Akreditacija? Akreditacija { get; set; }
    public List<Oblast>? Oblasti { get; set; }
    public Smer? Smer { get; set; }
    public List<Profesor>? Profesori { get; set; }
    public List<Asistent>? Asistenti { get; set; }
    public List<Student>? Studenti { get; set; }
}