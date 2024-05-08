using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
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

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiAdministratore(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Administratori.Count() / (float)brojRezultata);

        var admin = await _context.Administratori
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Administrator>
        {
            Response = admin,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Administrator>))]
    public async Task<ActionResult> VratiAdministratora(int id)
    {
        return Ok(await _context.Administratori.FindAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> DodajAdministratora([FromBody]Administrator admin)
    {
        await _context.Administratori.AddAsync(admin);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAdministratora), 
            new { id = admin.Id }, 
            admin);
    }

    [HttpPut]
    public async Task<ActionResult> AzurirajAdministratora([FromBody]Administrator admin)
    {
        var adminZaAzuriranje = HttpContext.Items["admin"] as Administrator;
        adminZaAzuriranje!.Ime=admin.Ime;
        adminZaAzuriranje.Email=admin.Email;
        adminZaAzuriranje.Password=admin.Password;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Administrator>))]
    public async Task<ActionResult> ObrisiAdministratora(int id)
    {
        var adminZaBrisanje = await _context.Administratori.FindAsync(id);
        _context.Administratori.Remove(adminZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(adminZaBrisanje);
    }

}
