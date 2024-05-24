using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.AsistentFilters;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AsistentController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AsistentController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiAsistente(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Asistenti.Count() / (float)brojRezultata);

        var asistenti = await _context.Asistenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Asistent>
        {
            Podaci = asistenti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult> VratiAsistenta(int id)
    {
        return Ok(await _context.Asistenti.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajAsistentaFilter))]
    public async Task<ActionResult> DodajAsistenta([FromBody]Asistent asistent)
    {
        _context.Asistenti.Add(asistent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAsistenta),
            new { id = asistent.Id },
            asistent
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajAsistentaFilter))]
    public async Task<ActionResult> AzurirajAsistenta([FromBody]Asistent asistent)
    {
        var asistentZaAzuriranje = HttpContext.Items["asistent"] as Asistent;
        asistentZaAzuriranje!.Ime = asistent.Ime;
        asistentZaAzuriranje.Prezime = asistent.Prezime;
        asistentZaAzuriranje.Email = asistent.Email;
        //asistentZaAzuriranje.Password = asistent.Password;
        asistentZaAzuriranje.Katedra = asistent.Katedra;
        asistentZaAzuriranje.Smerovi = asistent.Smerovi;
        asistentZaAzuriranje.Predmeti = asistent.Predmeti;

        await _context.SaveChangesAsync();
    
        return Ok(asistentZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult> ObrisiAsistenta(int id)
    {
        var asistentZaBrisanje = await _context.Asistenti.FindAsync(id);
        _context.Asistenti.Remove(asistentZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}