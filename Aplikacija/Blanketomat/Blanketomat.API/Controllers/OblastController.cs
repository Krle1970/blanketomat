using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.OblastFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OblastController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public OblastController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiOblasti(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Oblasti.Count() / (float)brojRezultata);

        var oblasti = await _context.Oblasti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Oblast>
        {
            Podaci = oblasti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Oblast>))]
    public async Task<ActionResult> VratiOblast(int id)
    {
        return Ok(await _context.Oblasti.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajOblastFilter))]
    public async Task<ActionResult> DodajOblast([FromBody]Oblast oblast)
    {
        _context.Oblasti.Add(oblast);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiOblast),
            new { id = oblast.Id },
            oblast
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajOblastFilter))]
    public async Task<ActionResult> AzurirajOblast([FromBody]Oblast oblast)
    {
        var oblastZaAzuriranje = HttpContext.Items["oblast"] as Oblast;
        oblastZaAzuriranje!.Naziv=oblast.Naziv;
        oblastZaAzuriranje.Pitanja=oblast.Pitanja;
        oblastZaAzuriranje.Zadaci=oblast.Zadaci;
        oblastZaAzuriranje.Blanketi=oblast.Blanketi;
       
        await _context.SaveChangesAsync();
        return Ok(oblastZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Oblast>))]
    public async Task<ActionResult> ObrisiOblast(int id)
    {
        var oblastZaBrisanje = await _context.Oblasti.FindAsync(id);
        _context.Oblasti.Remove(oblastZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}