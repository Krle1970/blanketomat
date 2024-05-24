using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.IspitniRokDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.IspitniRokFIlters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class IspitniRokController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public IspitniRokController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("ispitni-rokovi")]
    [TypeFilter(typeof(ValidateDbSetFilter<IspitniRok>))]
    public ActionResult<IEnumerable<IspitniRok>> VratiSveIspitneRokove()
    {
        return Ok(_context.IspitniRokovi);
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<IspitniRok>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<IspitniRok>>> VratiIspitneRokove(int page, int count)
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
    [TypeFilter(typeof(ValidateDbSetFilter<IspitniRok>))]
    [TypeFilter(typeof(ValidateIdFilter<IspitniRok>))]
    public ActionResult<IspitniRok> VratiIspitniRok(int id)
    {
        return Ok(HttpContext.Items["entity"] as IspitniRok);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDodajIspitniRokFilter))]
    public async Task<ActionResult> DodajIspitniRok([FromBody]DodajIspitniRokDTO noviIspitniRok)
    {
        var ispitniRok = new IspitniRok
        {
            Naziv = noviIspitniRok.Naziv
        };

        _context.IspitniRokovi.Add(ispitniRok);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiIspitniRok), 
            new { id = ispitniRok.Id }, 
            ispitniRok
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<IspitniRok>))]
    [TypeFilter(typeof(ValidateIdFilter<IspitniRok>))]
    [TypeFilter(typeof(ValidateAzurirajIspitniRokFilter))]
    public async Task<ActionResult<IspitniRok>> AzurirajIspitniRok(int id, [FromBody]AzurirajIspitniRokDTO ispitniRok)
    {
        // iz ValidateIdFilter-a
        var rokZaAzuriranje = HttpContext.Items["entity"] as IspitniRok;
        rokZaAzuriranje!.Naziv = ispitniRok.Naziv;
        
        if (ispitniRok.PonavljanjaIspitnihRokovaIds != null)
        {
            PonavljanjeIspitnogRoka? ponavljanjeIspitnogRoka;
            for (int i = 0; i < ispitniRok.PonavljanjaIspitnihRokovaIds.Count(); i++)
            {
                ponavljanjeIspitnogRoka = await _context.PonavljanjaIspitnihRokova.FindAsync(ispitniRok.PonavljanjaIspitnihRokovaIds[i]);
                if (ponavljanjeIspitnogRoka != null)
                {
                    if (!rokZaAzuriranje.Ponavljanja!.Contains(ponavljanjeIspitnogRoka))
                        rokZaAzuriranje.Ponavljanja.Add(ponavljanjeIspitnogRoka);
                }
            }
        }
       
        await _context.SaveChangesAsync();
        return Ok(rokZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<IspitniRok>))]
    [TypeFilter(typeof(ValidateIdFilter<IspitniRok>))]
    public async Task<ActionResult<string>> ObrisiIspitniRok(int id)
    {
        // iz ValidateDbFilter-a
        var IspitniRokZaBrisanje = HttpContext.Items["entity"] as IspitniRok;
        _context.IspitniRokovi.Remove(IspitniRokZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Ispitni rok uspešno obrisan");
    }
}