using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.AdministratorDTOs;
using Blanketomat.API.Filters.AdministratorFilters;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AdministratorController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AdministratorController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("administratori")]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    public async Task<ActionResult<List<Administrator>>> VratiSveAdministratore()
    {
        return Ok(await _context.Administratori.ToListAsync());
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Administrator>>> VratiAdministratore(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Administratori.Count() / (float)brojRezultata);

        var administratori = await _context.Administratori
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Administrator>
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
    public ActionResult<Administrator> VratiAdministratora(int id)
    {
        return Ok(HttpContext.Items["entity"]);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajAdministratoraFilter))]
    public async Task<ActionResult> DodajAdministratora([FromBody]AdministratorDTO administrator)
    {
        PasswordManager.CreatePasswordHash(administrator.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var admin = new Administrator
        {
            Ime = administrator.Ime,
            Prezime = administrator.Prezime,
            Email = administrator.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        _context.Administratori.Add(admin);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAdministratora), 
            new { id = admin.Id }, 
            admin
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    [TypeFilter(typeof(ValidateIdFilter<Administrator>))]
    [TypeFilter(typeof(ValidateAzurirajAdministratoraFilter<Administrator>))]
    public async Task<ActionResult<Administrator>> AzurirajAdministratora(int id, [FromBody]AdministratorDTO administrator)
    {
        // iz ValidateIdFilter-a
        var administratorZaAzuriranje = HttpContext.Items["entity"] as Administrator;

        if (administratorZaAzuriranje != null)
        {
            administratorZaAzuriranje.Ime = administrator.Ime;
            administratorZaAzuriranje.Prezime = administrator.Prezime;
            administratorZaAzuriranje.Email = administrator.Email;

            PasswordManager.CreatePasswordHash(administrator.Password, out byte[] newPasswordHash, out byte[] newPasswordSalt);
            administratorZaAzuriranje.PasswordHash = newPasswordHash;
            administratorZaAzuriranje.PasswordSalt = newPasswordSalt;
        }

        await _context.SaveChangesAsync();
        return Ok(administratorZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Administrator>))]
    [TypeFilter(typeof(ValidateIdFilter<Administrator>))]
    public async Task<ActionResult<string>> ObrisiAdministratora(int id)
    {
        // iz ValidateIdFilter-a
        var administratorZaBrisanje = HttpContext.Items["entity"] as Administrator;
        _context.Administratori.Remove(administratorZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Administrator uspešno obrisan");
    }
}