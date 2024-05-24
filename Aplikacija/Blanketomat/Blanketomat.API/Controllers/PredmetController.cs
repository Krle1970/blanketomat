using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PredmetFilters;
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

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiPredmete(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Predmeti.Count() / (float)brojRezultata);

        var predmeti = await _context.Predmeti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Predmet>
        {
            Podaci = predmeti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult> VratiPredmet(int id)
    {
        return Ok(await _context.Predmeti.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajPredmetFilter))]
    public async Task<ActionResult> DodajPredmet([FromBody]Predmet predmet)
    {
        _context.Predmeti.Add(predmet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPredmet), 
            new { id = predmet.Id }, 
            predmet
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajPredmetFilter))]
    public async Task<ActionResult> AzurirajPredmet([FromBody]Predmet predmet)
    {
        var predmetZaAzuriranje = HttpContext.Items["predmet"] as Predmet;
        predmetZaAzuriranje!.Naziv = predmet.Naziv;
        predmetZaAzuriranje.Godina = predmet.Godina;
        predmetZaAzuriranje.Akreditacija = predmet.Akreditacija;
        //predmetZaAzuriranje.Katedra = predmet.Katedra;
        predmetZaAzuriranje.Smer = predmet.Smer;
        predmetZaAzuriranje.Profesori = predmet.Profesori;
        predmetZaAzuriranje.Asistenti = predmet.Asistenti;
        predmetZaAzuriranje.Studenti = predmet.Studenti;

        await _context.SaveChangesAsync();
        return Ok(predmetZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult> ObrisiPredmet(int id)
    {
        var predmetZaBrisanje = await _context.Predmeti.FindAsync(id);
        _context.Predmeti.Remove(predmetZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}