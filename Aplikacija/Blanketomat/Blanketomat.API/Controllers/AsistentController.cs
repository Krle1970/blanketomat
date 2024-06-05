using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.AsistentDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Helper;
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

    [HttpGet("asistenti")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    public ActionResult<IEnumerable<Asistent>> VratiSveAsistente()
    {
        return Ok(_context.Asistenti);
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Asistent>>> VratiAsistente(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Asistenti.Count() / (float)brojRezultata);

        var asistenti = await _context.Asistenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Asistent>
        {
            Podaci = asistenti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public ActionResult<Asistent> VratiAsistenta(int id)
    {
        return Ok(HttpContext.Items["entity"] as Asistent);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult<Asistent>> AzurirajAsistenta(int id, [FromBody]AzurirajAsistentaDTO asistent)
    {
        // iz ValidateIdFilter-a
        var asistentZaAzuriranje = HttpContext.Items["entity"] as Asistent;

        PasswordManager.CreatePasswordHash(asistent.Password, out byte[] passwordHash, out byte[] passwordSalt);

        asistentZaAzuriranje!.Ime = asistent.Ime;
        asistentZaAzuriranje.Prezime = asistent.Prezime;
        asistentZaAzuriranje.Email = asistent.Email;
        asistentZaAzuriranje.PasswordHash = passwordHash;
        asistentZaAzuriranje.PasswordSalt = passwordSalt;
        asistentZaAzuriranje.Katedra = asistent.Katedra;
        asistentZaAzuriranje.Smerovi = asistent.Smerovi;
        asistentZaAzuriranje.Predmeti = asistent.Predmeti;
        asistentZaAzuriranje.LajkovaniKomentari = asistent.LajkovaniKomentari;
        asistentZaAzuriranje.LajkovaniOdgovori = asistent.LajkovaniOdgovori;

        await _context.SaveChangesAsync();
        return Ok(asistentZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult<string>> ObrisiAsistenta(int id)
    {
        var asistentZaBrisanje = HttpContext.Items["entity"] as Asistent;
        _context.Asistenti.Remove(asistentZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Asistent uspešno obrisan");
    }
}