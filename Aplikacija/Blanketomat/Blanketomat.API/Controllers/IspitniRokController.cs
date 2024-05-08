using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters;
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
    [HttpGet("{page}/{count}")]
    public async Task<ActionResult> VratiIspitniRok(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.IspitniRokovi.Count() / (float)brojRezultata);

        var iRok = await _context.IspitniRokovi
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<IspitniRok>
        {
            Response = iRok,
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
    public async Task<ActionResult> DodajIspitniRok([FromBody]IspitniRok irok)
    {
        await _context.IspitniRokovi.AddAsync(irok);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiIspitniRok), 
            new { id = irok.Id }, 
            irok);
    }

    [HttpPut]
    
    public async Task<ActionResult> AzurirajIspitniRok([FromBody]IspitniRok irok)
    {
        var rokZaAzuriranje = HttpContext.Items["irok"] as IspitniRok;
        rokZaAzuriranje!.Naziv=irok.Naziv;
        rokZaAzuriranje.Ponavljanja=irok.Ponavljanja;
       
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<IspitniRok>))]
    public async Task<ActionResult> ObrisiIspitniRok(int id)
    {
        var IspitniRokZaBrisanje = await _context.IspitniRokovi.FindAsync(id);
        _context.IspitniRokovi.Remove(IspitniRokZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok(IspitniRokZaBrisanje);
    }


}
