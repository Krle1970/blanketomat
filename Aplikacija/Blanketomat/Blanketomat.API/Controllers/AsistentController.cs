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
    public async Task<ActionResult<Asistent>> AzurirajAsistenta(int id, [FromBody] AsistentDTO asistentDTO)
    {
        var asistentZaAzuriranje = await _context.Asistenti.FindAsync(id);

        if (asistentZaAzuriranje == null)
        {
            return NotFound();
        }

        PasswordManager.CreatePasswordHash(asistentDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

        asistentZaAzuriranje.Ime = asistentDTO.Ime;
        asistentZaAzuriranje.Prezime = asistentDTO.Prezime;
        asistentZaAzuriranje.Email = asistentDTO.Email;
        asistentZaAzuriranje.PasswordHash = passwordHash;
        asistentZaAzuriranje.PasswordSalt = passwordSalt;

        if (asistentDTO.Katedra != null)
        {
            asistentZaAzuriranje.Katedra = asistentDTO.Katedra;
        }
        if (asistentDTO.Smerovi != null)
        {
            asistentZaAzuriranje.Smerovi = asistentDTO.Smerovi;
        }
        if (asistentDTO.Predmeti != null)
        {
            asistentZaAzuriranje.Predmeti = asistentDTO.Predmeti;
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok(asistentZaAzuriranje);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Asistent>))]
    [TypeFilter(typeof(ValidateIdFilter<Asistent>))]
    public async Task<ActionResult<string>> ObrisiAsistenta(int id)
    {
        var asistentZaBrisanje = HttpContext.Items["entity"] as Asistent;
        _context.Asistenti.Remove(asistentZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Asistent uspe�no obrisan");
    }
}