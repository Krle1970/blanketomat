using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.IspitniRokFIlters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IspitniRokController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public IspitniRokController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> VratiSveIspitneRokove()
    {
        return Ok(await _context.IspitniRokovi.ToListAsync());
    }

    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiIspitneRokove(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.IspitniRokovi.Count() / (float)brojRezultata);

        var ispitniRok = await _context.IspitniRokovi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<IspitniRok>
        {
            Podaci = ispitniRok,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<IspitniRok>))]
    public async Task<ActionResult> VratiIspitniRok(int id)
    {
        return Ok(await _context.IspitniRokovi.FindAsync(id));
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajIspitniRokFilter))]
    public async Task<ActionResult> DodajIspitniRok([FromBody]IspitniRok ispitniRok)
    {
        _context.IspitniRokovi.Add(ispitniRok);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiIspitniRok), 
            new { id = ispitniRok.Id }, 
            ispitniRok
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateAzurirajIspitniRokFilter))]
    public async Task<ActionResult> AzurirajIspitniRok([FromBody]IspitniRok ispitniRok)
    {
        var rokZaAzuriranje = HttpContext.Items["ispitniRok"] as IspitniRok;
        rokZaAzuriranje!.Naziv = ispitniRok.Naziv;
        rokZaAzuriranje.Ponavljanja = ispitniRok.Ponavljanja;
       
        await _context.SaveChangesAsync();
        return Ok(rokZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<IspitniRok>))]
    public async Task<ActionResult> ObrisiIspitniRok(int id)
    {
        var IspitniRokZaBrisanje = await _context.IspitniRokovi.FindAsync(id);
        _context.IspitniRokovi.Remove(IspitniRokZaBrisanje!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}