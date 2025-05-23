﻿using Blanketomat.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.DTOs.PitanjeDTOs;

public class PitanjeDTO
{
    [MinLength(5, ErrorMessage = "Tekst pitanja mora imati minimum 5 karaktera")]
    [MaxLength(500, ErrorMessage = "Tekst pitanja moze imati maksimalno 500 karaktera")]
    public required string Tekst { get; set; }
    public List<Slika>? Slike { get; set; }
    public Oblast? Oblast { get; set; }
}