using Blanketomat.API.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.BlanketDTOs;

// mora da postoji zbog validacija za Tip i Kategoriju blanketa (razliciti DTO objekti
// se prosledjuju u DodajBlanket i AzurirajBlanket funkcijama)
public class BlanketDTO
{
    [MinLength(3, ErrorMessage = "Tip blanketa mora imati minimalno 3 karaktera")]
    [MaxLength(20, ErrorMessage = "Tip blanketa moze imati maksimalno 20 karaktera")]
    [TipBlanketaValidation]
    public required string Tip { get; set; }

    // Kategorija - I kolok, II kolok, Ispit...
    [MinLength(5, ErrorMessage = "Kategorija blanketa mora imati minimalno 5 karaktera")]
    [MaxLength(50, ErrorMessage = "Kategorija blanketa moze imati maksimalno 50 karaktera")]
    [KategorijaBlanketaValidation]
    public required string Kategorija { get; set; }

    [MinLength(3, ErrorMessage = "Putanja mora imati minimalno 3 karaktera")]
    public required string Putanja { get; set; }
    public int[]? SlikeIds { get; set; }
    public int? PredmetId { get; set; }
    public int? PonavljanjeIspitnogRokaId { get; set; }
    public int[]? PitanjaIds { get; set; }
    public int[]? ZadaciIds { get; set; }
}