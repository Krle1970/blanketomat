﻿using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.OblastDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Filters.PredmetFilters;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PredmetController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public PredmetController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PagingResponseDTO<Predmet>>> VratiPredmete(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Predmeti.Count() / (float)brojRezultata);

        var predmeti = await _context.Predmeti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Predmet>
        {
            Podaci = predmeti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }
    [HttpGet("predmeti")]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    public ActionResult<IEnumerable<Predmet>> VratiSvePredmete()
    {
        return Ok(_context.Predmeti);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public ActionResult<Predmet> VratiPredmet(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Predmet);
    }

     [HttpPost("{predmetId}/oblasti")]
        public ActionResult<Oblast> AddOblastToPredmet(int predmetId, [FromBody] Oblast newOblast)
        {
            var predmet = _context.Predmeti.Include(p => p.Oblasti).FirstOrDefault(p => p.Id == predmetId);

            if (predmet == null)
            {
                return NotFound("Predmet nije pronađen.");
            }

            newOblast.Predmet = predmet;
            _context.Oblasti.Add(newOblast);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOblastiByPredmet), new { predmetId = predmet.Id }, newOblast);
        }
        [HttpGet("{predmetId}/oblasti")]
        public ActionResult<IEnumerable<OblastDTO>> GetOblastiByPredmet(int predmetId)
        {
            var predmet = _context.Predmeti
                .Include(p => p.Oblasti)
                .FirstOrDefault(p => p.Id == predmetId);

            if (predmet == null)
            {
                return NotFound("Predmet nije pronađen.");
            }

            var oblasti = predmet.Oblasti?.Select(o => new OblastDTO
            {
                Id = o.Id,
                Naziv = o.Naziv
            }).ToList();

            if (oblasti == null || !oblasti.Any())
            {
                return NotFound("Nema oblasti za dati predmet.");
            }

            return Ok(oblasti);
        }

    [HttpPost]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    //[TypeFilter(typeof(ValidateDodajPredmetFilter))]
    public async Task<ActionResult> DodajPredmet([FromBody]DodajPredmetDTO noviPredmet)
    {
        Predmet predmet = new Predmet
        {
            Naziv = noviPredmet.Naziv,
            Godina = noviPredmet.Godina,
        };

        _context.Predmeti.Add(predmet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(VratiPredmet), 
            new { id = predmet.Id }, 
            predmet
            );
    }

    [HttpPut]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult<Predmet>> AzurirajPredmet(int id, [FromBody]AzurirajPredmetDTO predmet)
    {
        // iz ValidateIdFilter-a
        var predmetZaAzuriranje = HttpContext.Items["entity"] as Predmet;
        predmetZaAzuriranje!.Naziv = predmet.Naziv;
        predmetZaAzuriranje.Godina = predmet.Godina;
        predmetZaAzuriranje.Akreditacija =predmet.Akreditacija;
        predmetZaAzuriranje.Oblasti = predmet.Oblasti;
        predmetZaAzuriranje.Smer = predmet.Smer;
        predmetZaAzuriranje.Profesori = predmet.Profesori;
        predmetZaAzuriranje.Asistenti = predmet.Asistenti;
        predmetZaAzuriranje.Studenti = predmet.Studenti;
        predmetZaAzuriranje.Blanketi = predmet.Blanketi;

        await _context.SaveChangesAsync();
        return Ok(predmetZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Predmet>))]
    [TypeFilter(typeof(ValidateIdFilter<Predmet>))]
    public async Task<ActionResult<string>> ObrisiPredmet(int id)
    {
        // Iz ValidateIdFilter-a
        var predmetZaBrisanje = HttpContext.Items["entity"] as Predmet;

        if (predmetZaBrisanje == null)
        {
            return NotFound("Predmet nije pronađen");
        }

        // Učitajte povezane oblasti
        _context.Entry(predmetZaBrisanje).Collection(p => p.Oblasti).Load();

        if (predmetZaBrisanje.Oblasti != null)
        {
            foreach (var oblast in predmetZaBrisanje.Oblasti)
            {
               
                _context.Entry(oblast).Collection(o => o.Zadaci).Load();
                if (oblast.Zadaci != null)
                {
                    _context.Zadaci.RemoveRange(oblast.Zadaci);
                }

               
                _context.Entry(oblast).Collection(o => o.Pitanja).Load();
                if (oblast.Pitanja != null)
                {
                    _context.Pitanja.RemoveRange(oblast.Pitanja);
                }

                // Učit i uklonipovezane podoblasti za svaku oblast ako postoje, kod nas je retko to
                _context.Entry(oblast).Collection(o => o.Podoblasti).Load();
                if (oblast.Podoblasti != null)
                {
                    _context.Podoblasti.RemoveRange(oblast.Podoblasti);
                }
            }

            // Ukloni povezane oblasti
            _context.Oblasti.RemoveRange(predmetZaBrisanje.Oblasti);
        }

        // Ukloni povezane blankete
        _context.Entry(predmetZaBrisanje).Collection(p => p.Blanketi).Load();
        if (predmetZaBrisanje.Blanketi != null)
        {
            _context.Blanketi.RemoveRange(predmetZaBrisanje.Blanketi);
        }

        // Uklon predmet
        _context.Predmeti.Remove(predmetZaBrisanje);
        await _context.SaveChangesAsync();

        return Ok("Predmet, povezane oblasti, zadaci, pitanja, podoblasti i blanketi su uspešno obrisani");
    }
}