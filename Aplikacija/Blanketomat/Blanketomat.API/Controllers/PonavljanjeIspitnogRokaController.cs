using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.PonavljanjeIspitnogRokaDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PonavljanjeIspitnogRokaFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PonavljanjeIspitnogRokaController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PonavljanjeIspitnogRokaController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<PonavljanjeIspitnogRoka>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<PonavljanjeIspitnogRoka>>> VratiPonavljanjaIspitnihRokova(int page, int count)
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
    [TypeFilter(typeof(ValidateDbSetFilter<PonavljanjeIspitnogRoka>))]
    [TypeFilter(typeof(ValidateIdFilter<PonavljanjeIspitnogRoka>))]
    public ActionResult<PonavljanjeIspitnogRoka> VratiPonavljanjeIspitnogRoka(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as PonavljanjeIspitnogRoka);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<PonavljanjeIspitnogRoka>))]
    [TypeFilter(typeof(ValidateDodajPonavljanjeIspitnogRokaFilter))]
    public async Task<ActionResult> DodajPonavljanjeRoka([FromBody] DodajPonavljanjeIspitnogRokaDTO novoPonavljanjeIspitnogRoka)
    {
        PonavljanjeIspitnogRoka ponavljanjeIspitnogRoka = new PonavljanjeIspitnogRoka
        {
            Naziv = novoPonavljanjeIspitnogRoka.Naziv,
            Datum = novoPonavljanjeIspitnogRoka.Datum
        };

        if (novoPonavljanjeIspitnogRoka.IspitniRokId != null)
        {
            IspitniRok? ispitniRok = await _context.IspitniRokovi.FindAsync(novoPonavljanjeIspitnogRoka.IspitniRokId);
            if (ispitniRok != null)
            {
                ponavljanjeIspitnogRoka.IspitniRok = ispitniRok;
            }
        }

        _context.PonavljanjaIspitnihRokova.Add(ponavljanjeIspitnogRoka);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPonavljanjeIspitnogRoka),
            new { id = ponavljanjeIspitnogRoka.Id },
            ponavljanjeIspitnogRoka
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<PonavljanjeIspitnogRoka>))]
    [TypeFilter(typeof(ValidateIdFilter<PonavljanjeIspitnogRoka>))]
    public async Task<ActionResult<PonavljanjeIspitnogRoka>> AzurirajPonavljanjeIspitnogRoka(int id, [FromBody] AzurirajPonavljanjeIspitnogRokaDTO ponavljanjeIspitnogRoka)
    {
        // id ValidateIdFilter-a
        var ponavljanjeIspitnogRokaZaAzuriranje = HttpContext.Items["entity"] as PonavljanjeIspitnogRoka;
        ponavljanjeIspitnogRokaZaAzuriranje!.Naziv = ponavljanjeIspitnogRoka.Naziv;
        ponavljanjeIspitnogRokaZaAzuriranje.Datum = ponavljanjeIspitnogRoka.Datum;

        if (ponavljanjeIspitnogRoka.IspitniRokId != null)
        {
            IspitniRok? ispitniRok = await _context.IspitniRokovi.FindAsync(ponavljanjeIspitnogRoka.IspitniRokId);
            if (ispitniRok != null)
            {
                ponavljanjeIspitnogRokaZaAzuriranje.IspitniRok = ispitniRok;
            }
        }

        if (ponavljanjeIspitnogRoka.BlanketiIds != null)
        {
            Blanket? blanket;
            for (int i = 0; i < ponavljanjeIspitnogRoka.BlanketiIds.Count(); i++)
            {
                blanket = await _context.Blanketi.FindAsync(ponavljanjeIspitnogRoka.BlanketiIds[i]);
                if (blanket != null)
                {
                    if (!ponavljanjeIspitnogRokaZaAzuriranje.Blanketi!.Contains(blanket))
                        ponavljanjeIspitnogRokaZaAzuriranje.Blanketi.Add(blanket);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ponavljanjeIspitnogRokaZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<PonavljanjeIspitnogRoka>))]
    [TypeFilter(typeof(ValidateIdFilter<PonavljanjeIspitnogRoka>))]
    public async Task<ActionResult<string>> ObrisiPonavljanjeIspitnogRoka(int id)
    {
        // iz ValidateIdFilter-a
        var ponavljanjeIspitnogRokaZaBrisanje = HttpContext.Items["entity"] as PonavljanjeIspitnogRoka;
        _context.PonavljanjaIspitnihRokova.Remove(ponavljanjeIspitnogRokaZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Ponavljanje ispitnog roka uspešno obrisano");
    }
}