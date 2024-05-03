using Blanketomat.API.Context;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredmetController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PredmetController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{start}/{limit}")]
    public async Task<ActionResult> VratiPredmete(int start, int limit)
    {
        return Ok(await _context.Predmeti.Skip(start).Take(limit).ToListAsync());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult> VratiPredmet(int id)
    {
        return Ok(await _context.Predmeti.FindAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> DodajPredmet([FromBody]Predmet predmet)
    {
        await _context.Predmeti.AddAsync(predmet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPredmet), 
            new { id = predmet.Id }, 
            predmet);
    }

    [HttpPut]
    public async Task<ActionResult> AzurirajPredmet([FromBody]Predmet predmet)
    {
        var predmetZaAzuriranje = await _context.Predmeti.FindAsync(predmet.Id);
        predmetZaAzuriranje.Naziv = predmet.Naziv;
        predmetZaAzuriranje.Godina = predmet.Godina;
        predmetZaAzuriranje.Akreditacija = predmet.Akreditacija;
        predmetZaAzuriranje.Smer = predmet.Smer;
        predmetZaAzuriranje.Profesori = predmet.Profesori;
        predmetZaAzuriranje.Asistenti = predmet.Asistenti;
        predmetZaAzuriranje.Studenti = predmet.Studenti;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> ObrisiPredmet(int id)
    {
        var predmetZaBrisanje = await _context.Predmeti.FindAsync(id);
        _context.Predmeti.Remove(predmetZaBrisanje);
        await _context.SaveChangesAsync();

        return Ok(predmetZaBrisanje);
    }
}