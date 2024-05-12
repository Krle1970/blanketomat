using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.AdministratorFilters;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdministratorController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AdministratorController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    public async Task<ActionResult> VratiSveAdministratore()
    {
        return Ok(await _context.Administratori.ToListAsync());
    }

    [HttpGet("{page}/{count}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    public async Task<ActionResult> VratiAdministratore(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Administratori.Count() / (float)brojRezultata);

        var administratori = await _context.Administratori
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Administrator>
        {
            Podaci = administratori,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    [TypeFilter(typeof(ValidateIdFilter<Administrator>))]
    public async Task<ActionResult> VratiAdministratora(int id)
    {
        return Ok(await _context.Administratori.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajAdministratoraFilter))]
    public async Task<ActionResult> DodajAdministratora([FromBody]Administrator administrator)
    {
        _context.Administratori.Add(administrator);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAdministratora), 
            new { id = administrator.Id }, 
            administrator
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajAdministratoraFilter))]
    public async Task<ActionResult> AzurirajAdministratora([FromBody]Administrator administrator)
    {
        var administratorZaAzuriranje = HttpContext.Items["administrator"] as Administrator;
        administratorZaAzuriranje!.Ime = administrator.Ime;
        administratorZaAzuriranje.Email = administrator.Email;
        administratorZaAzuriranje.Password = administrator.Password;

        await _context.SaveChangesAsync();
        return Ok(administratorZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    [TypeFilter(typeof(ValidateIdFilter<Administrator>))]
    public async Task<ActionResult> ObrisiAdministratora(int id)
    {
        var administratorZaBrisanje = await _context.Administratori.FindAsync(id);
        _context.Administratori.Remove(administratorZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}