using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlanketController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public BlanketController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiBlankete(int page, int count)
    {
        
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Blanketi.Count() / (float)brojRezultata);

        var blanketi = await _context.Blanketi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Blanket>
        {
            Podaci = blanketi,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
    public async Task<ActionResult> VratiBlanket(int id)
    {
        return Ok(await _context.Blanketi.FindAsync(id));
    }
      
    [HttpPost]
    [TypeFilter(typeof(ValidateDodajBlanketFilter))]
    public async Task<ActionResult> DodajBlanket([FromBody]Blanket blanket) 
    {
        _context.Blanketi.Add(blanket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiBlanket),
            new { id = blanket.Id },
            blanket
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajBlanketFilter))]
    public async Task<ActionResult> AzurirajBlanket([FromBody]Blanket blanket)
    {
        var blanketZaAzuriranje = HttpContext.Items["blanket"] as Blanket;
        blanketZaAzuriranje!.Tip = blanket.Tip;
        blanketZaAzuriranje.Kategorija = blanket.Kategorija;
        blanketZaAzuriranje.IspitniRok = blanket.IspitniRok;
        blanketZaAzuriranje.Pitanja = blanket.Pitanja;
        blanketZaAzuriranje.Zadaci = blanket.Zadaci;

        await _context.SaveChangesAsync();
        return Ok(blanketZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Blanket>))]
    public async Task<ActionResult> ObrisiBlanket(int id)
    {
        var blanketZaBrisanje = await _context.Blanketi.FindAsync(id);
        _context.Blanketi.Remove(blanketZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}