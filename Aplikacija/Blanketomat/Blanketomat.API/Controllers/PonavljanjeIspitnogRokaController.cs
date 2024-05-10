using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PonavljanjeIspitnogRokaFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PonavljanjeIspitnogRokaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PonavljanjeIspitnogRokaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiPonavljanjaIspitnihRokova(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.PonavljanjaIspitnihRokova.Count() / (float)brojRezultata);

        var ponavljanjaIspitnihRokova = await _context.PonavljanjaIspitnihRokova
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<PonavljanjeIspitnogRoka>
        {
            Podaci = ponavljanjaIspitnihRokova,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<PonavljanjeIspitnogRoka>))]
    public async Task<ActionResult> VratiPonavljanjeIspitnogRoka(int id)
    {
        return Ok(await _context.PonavljanjaIspitnihRokova.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajPonavljanjeIspitnogRokaFilter))]
    public async Task<ActionResult> DodajPonavljanjeRoka([FromBody] PonavljanjeIspitnogRoka ponavljanjeIspitnogRoka)
    {
        _context.PonavljanjaIspitnihRokova.Add(ponavljanjeIspitnogRoka);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPonavljanjeIspitnogRoka),
            new { id = ponavljanjeIspitnogRoka.Id },
            ponavljanjeIspitnogRoka
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajPonavljanjeIspitnogRoka))]
    public async Task<ActionResult> AzurirajPonavljanjeIspitnogRoka([FromBody] PonavljanjeIspitnogRoka ponavljanjeIspitnogRoka)
    {
        var ponavljanjeIspitnogRokaZaAzuriranje = HttpContext.Items["ponavljanjeIspitnogRoka"] as PonavljanjeIspitnogRoka;
        ponavljanjeIspitnogRokaZaAzuriranje!.Datum = ponavljanjeIspitnogRoka.Datum;
        ponavljanjeIspitnogRokaZaAzuriranje.IspitniRok = ponavljanjeIspitnogRoka.IspitniRok;
        ponavljanjeIspitnogRokaZaAzuriranje.Blanketi = ponavljanjeIspitnogRoka.Blanketi;

        await _context.SaveChangesAsync();
        return Ok(ponavljanjeIspitnogRokaZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<PonavljanjeIspitnogRoka>))]
    public async Task<ActionResult> ObrisiPonavljanjeIspitnogRoka(int id)
    {
        var ponavljanjeIspitnogRokaZaBrisanje = await _context.PonavljanjaIspitnihRokova.FindAsync(id);
        _context.PonavljanjaIspitnihRokova.Remove(ponavljanjeIspitnogRokaZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}