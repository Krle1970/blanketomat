using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.ProfesorDTO;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProfesorController : ControllerBase
{       
     private readonly BlanketomatContext _context;

    public ProfesorController (BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Profesor>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Profesor>>> VratiProfesore(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Profesori.Count() / (float)brojRezultata);

        var profesori = await _context.Profesori
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Profesor>
        {
            Podaci = profesori,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Profesor>))]
    [TypeFilter(typeof(ValidateIdFilter<Profesor>))]
    public ActionResult<Profesor> VratiProfesora(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Profesor);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Profesor>))]
    public async Task<ActionResult<Profesor>> AzurirajProfesora(int id, [FromBody]AzurirajProfesoraDTO profesor)
    {
        // iz ValidateIdFilter-a
        var profesorZaAzuriranje = HttpContext.Items["entity"] as Profesor;

        PasswordManager.CreatePasswordHash(profesor.Password, out byte[] passwordHash, out byte[] passwordSalt);

        profesorZaAzuriranje!.Ime = profesor.Ime;
        profesorZaAzuriranje.Prezime = profesor.Prezime;
        profesorZaAzuriranje.Email = profesor.Email;
        profesorZaAzuriranje.PasswordHash = passwordHash;
        profesorZaAzuriranje.PasswordSalt = passwordSalt;
        profesorZaAzuriranje.Smerovi = profesor.Smerovi;
        profesorZaAzuriranje.Predmeti = profesor.Predmeti;
        profesorZaAzuriranje.Katedra = profesor.Katedra;
        profesorZaAzuriranje.LajkovaniKomentari = profesor.LajkovaniKomentari;
        profesorZaAzuriranje.LajkovaniOdgovori = profesor.LajkovaniOdgovori;
        
        await _context.SaveChangesAsync();
    
        return Ok(profesorZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Profesor>))]
    [TypeFilter(typeof(ValidateIdFilter<Profesor>))]
    public async Task<ActionResult> ObrisiProfesora(int id)
    {
        // iz ValidateIdFilter-a
        var profesorZaBrisanje = HttpContext.Items["entity"] as Profesor;
        _context.Profesori.Remove(profesorZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Profesor uspešno obrisan");
    }
}