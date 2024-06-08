using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.ZadatakDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.ZadatakFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ZadatakController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public ZadatakController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Zadatak>>> VratiZadatke(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Zadaci.Count() / (float)brojRezultata);

        var zadaci = await _context.Zadaci
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Zadatak>
        {
            Podaci = zadaci,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public ActionResult<Zadatak> VratiZadatak(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Zadatak);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateDodajZadatakFilter))]
    public async Task<ActionResult> DodajZadatak([FromBody] DodajZadatakDTO noviZadatak)
    {
        Zadatak zadatak = new Zadatak
        {
            Tekst = noviZadatak.Tekst,
            Slika = noviZadatak.Slike,
            Oblast = noviZadatak.Oblast,
            Podoblast = noviZadatak.Podoblasti,
            Blanketi = noviZadatak.Blanketi
        };

        _context.Zadaci.Add(zadatak);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiZadatak),
            new { id = zadatak.Id },
            zadatak
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public async Task<ActionResult<Zadatak>> AzurirajZadatak(int id, [FromBody] AzurirajZadatakDTO zadatak)
    {
        // iz ValidateIdFilter-a
        var zadatakZaAzuriranje = HttpContext.Items["entity"] as Zadatak;
        zadatakZaAzuriranje!.Tekst = zadatak.Tekst;
        zadatakZaAzuriranje.Slika = zadatak.Slike;
        zadatakZaAzuriranje.Oblast = zadatak.Oblast;
        zadatakZaAzuriranje.Podoblast = zadatak.Podoblasti;
        zadatakZaAzuriranje.Blanketi = zadatak.Blanketi;

        await _context.SaveChangesAsync();
        return Ok(zadatakZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Zadatak>))]
    [TypeFilter(typeof(ValidateIdFilter<Zadatak>))]
    public async Task<ActionResult<string>> ObrisiZadatak(int id)
    {
        var zadatakZaBrisanje = HttpContext.Items["entity"] as Zadatak;
        _context.Zadaci.Remove(zadatakZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Zadatak uspešno obrisan");
    }
    [HttpPost("dodajZadatak")]
public async Task<IActionResult> DodajZadatak([FromBody] ZadatakDTO zadatakDTO)
{
    if (!ModelState.IsValid)
    {
        var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        return BadRequest(new { Errors = errorList });
    }

    var zadatak = new Zadatak
    {
        Tekst = zadatakDTO.Tekst,
        Oblast = zadatakDTO.Oblast != null ? await _context.Oblasti.FindAsync(zadatakDTO.Oblast.Id) : null,
        Slika = zadatakDTO.Slike?.ToList()
       
    };

    _context.Zadaci.Add(zadatak);
    await _context.SaveChangesAsync();

    return Ok(zadatak);
}
}