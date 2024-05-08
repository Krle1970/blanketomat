
using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
 public class AkreditacijaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AkreditacijaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiAkreditacije(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Akreditacije.Count() / (float)brojRezultata);

        var akred = await _context.Akreditacije
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Akreditacija>
        {
            Response = akred,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> VratiAkreditaciju(int id)
    {
        return Ok(await _context.Akreditacije.FindAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> DodajAkreditaciju([FromBody]Akreditacija akred)
    {
        await _context.Akreditacije.AddAsync(akred);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiAkreditaciju), 
            new { id = akred.Id }, 
            akred);
    }

    [HttpPut]
    public async Task<ActionResult> AzurirajAkreditaciju([FromBody]Akreditacija akred)
    {
        var ZaAzuriranje = HttpContext.Items["akred"] as Akreditacija;
        
        ZaAzuriranje!.Naziv=akred.Naziv;
        ZaAzuriranje.Predmeti=akred.Predmeti;
        ZaAzuriranje.Studenti=akred.Studenti;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Akreditacija>))]
    public async Task<ActionResult> ObrisiAkreditaciju(int id)
    {
        var akredZaBrisanje = await _context.Akreditacije.FindAsync(id);
        _context.Akreditacije.Remove(akredZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(akredZaBrisanje);
    }

}
