﻿using Blanketomat.API.Context;
using Blanketomat.API.DTOs.KatedraDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.KatedraFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class KatedraController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public KatedraController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet("katedre")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    public ActionResult<IEnumerable<Katedra>> VratiSveKatedre()
    {
        return Ok(_context.Katedre);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public ActionResult<Katedra> VratiKatedru(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Katedra);
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateDodajKatedruFilter))]
    public async Task<ActionResult> DodajKatedru([FromBody] DodajKatedruDTO novaKatedra)
    {
        Katedra katedra = new Katedra
        {
            Naziv = novaKatedra.Naziv,
            Smerovi = novaKatedra.Smerovi,
            Profesori = novaKatedra.Profesori,
            Asistenti = novaKatedra.Asistenti
        };

        _context.Katedre.Add(katedra);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiKatedru),
            new { id = katedra.Id },
            katedra
            );
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public async Task<ActionResult<Katedra>> AzurirajKatedru(int id, [FromBody] AzurirajKatedruDTO katedra)
    {
        // iz ValidateIdFilter-a
        var katedraZaAzuriranje = HttpContext.Items["entity"] as Katedra;

        katedraZaAzuriranje!.Naziv = katedra.Naziv;
        katedraZaAzuriranje.Smerovi = katedra.Smerovi;
        katedraZaAzuriranje.Profesori = katedra.Profesori;
        katedraZaAzuriranje.Asistenti = katedra.Asistenti;

        await _context.SaveChangesAsync();
        return Ok(katedraZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Katedra>))]
    [TypeFilter(typeof(ValidateIdFilter<Katedra>))]
    public async Task<ActionResult<string>> ObrisiKatedru(int id)
    {
       try
    {
        var katedra = await _context.Katedre.FindAsync(id);
        if (katedra == null)
        {
            return NotFound();
        }

        // Ažuriraj sve profesore koji referenciraju ovu katedru
        var profesori = _context.Profesori.Where(p => p.Katedra != null && p.Katedra.Id == id).ToList();
        if (profesori != null)
        {
            foreach (var profesor in profesori)
            {
                if (profesor != null)
                {
                    profesor.Katedra = null; // Uklanjanje reference na katedru
                }
            }
        } 
       

        await _context.SaveChangesAsync();

        _context.Katedre.Remove(katedra);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    catch (DbUpdateException ex)
    {
        // Logovanje greške
        Console.WriteLine(ex.InnerException?.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data.");
    }
    }
}