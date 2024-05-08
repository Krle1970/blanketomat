using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;
  
[Route("api/[controller]")]
[ApiController]
public class ProfesorController : ControllerBase
{       
     private readonly BlanketomatContext _context;

    public ProfesorController (BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiProfesore(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Profesori.Count() / (float)brojRezultata);

        var profesori = await _context.Profesori
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Profesor>
        {
            Response = profesori,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Profesor>))]
    public async Task<ActionResult> VratiProfesora(int id)
    {
        return Ok(await _context.Profesori.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajProfesoraFilter))]
    public async Task<ActionResult> DodajProfesora([FromBody] Profesor profesor)
    {
        await _context.Profesori.AddAsync(profesor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiProfesora),
            new { id = profesor.Id },
            profesor
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajProfesoraFilter))]
    public async Task<ActionResult> AzurirajProfesora([FromBody]Profesor profesor)
    {
        var profesorZaAzuriranje = HttpContext.Items["profesor"] as Profesor;
        profesorZaAzuriranje!.Ime = profesor.Ime;
        profesorZaAzuriranje.Prezime = profesor.Prezime;
        profesorZaAzuriranje.Email = profesor.Email;
        profesorZaAzuriranje.Password = profesor.Password;
        profesorZaAzuriranje.Smer = profesor.Smer;
        profesorZaAzuriranje.Predmeti = profesor.Predmeti;

        await _context.SaveChangesAsync();
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Profesor>))]
    public async Task<ActionResult> ObrisiProfesora(int id)
    {
        var profesorZaBrisanje = await _context.Studenti.FindAsync(id);
        _context.Studenti.Remove(profesorZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(profesorZaBrisanje);
    }
}