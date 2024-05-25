using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.DTOs.StudentDTOs;
using Blanketomat.API.Filters.GenericFilters;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public StudentController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpGet]
    [TypeFilter(typeof(ValidateDbSetFilter<Student>))]
    [ValidatePaginationFilter]
    public async Task<ActionResult<PaginationResponseDTO<Student>>> VratiStudente(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Studenti.Count() / (float)brojRezultata);

        var studenti = await _context.Studenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PaginationResponseDTO<Student>
        {
            Podaci = studenti,
            BrojStranica = (int)brojStranica,
            TrenutnaStranica = page
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public ActionResult<Student> VratiStudenta(int id)
    {
        // iz ValidateIdFilter-a
        return Ok(HttpContext.Items["entity"] as Student);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Student>))]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public async Task<ActionResult> AzurirajStudenta(int id, [FromBody]AzurirajStudentaDTO student)
    {
        // iz ValidateIdFilter-a
        var studentZaAzuriranje = HttpContext.Items["entity"] as Student;

        PasswordManager.CreatePasswordHash(student.Password, out byte[] passwordHash, out byte[] passwordSalt);

        studentZaAzuriranje!.Ime = student.Ime;
        studentZaAzuriranje.Prezime = student.Prezime;
        studentZaAzuriranje.Email = student.Email;
        studentZaAzuriranje.PasswordHash = passwordHash;
        studentZaAzuriranje.PasswordSalt = passwordSalt;
        
        if (student.AkreditacijaId != null)
        {
            Akreditacija? akreditacija = await _context.Akreditacije.FindAsync(student.AkreditacijaId);
            if (akreditacija != null)
            {
                studentZaAzuriranje.Akreditacija = akreditacija;
            }
        }

        if (student.SmerId != null)
        {
            Smer? smer = await _context.Smerovi.FindAsync(student.SmerId);
            if (smer != null)
            {
                studentZaAzuriranje.Smer = smer;
            }
        }

        if (student.PredmetiIds != null)
        {
            Predmet? predmet;
            for (int i = 0; i < student.PredmetiIds.Count(); i++)
            {
                predmet = await _context.Predmeti.FindAsync(student.PredmetiIds[i]);
                if (predmet != null)
                {
                    if (!studentZaAzuriranje.Predmeti!.Contains(predmet))
                        studentZaAzuriranje.Predmeti.Add(predmet);
                }
            }
        }

        if (student.PostavljeniKomentariIds != null)
        {
            Komentar? komentar;
            for (int i = 0; i < student.PostavljeniKomentariIds.Count(); i++)
            {
                komentar = await _context.Komentari.FindAsync(student.PostavljeniKomentariIds[i]);
                if (komentar != null)
                {
                    if (!studentZaAzuriranje.PostavljeniKomentari!.Contains(komentar))
                        studentZaAzuriranje.PostavljeniKomentari.Add(komentar);
                }
            }
        }

        if (student.PostavljeniOdgovoriIds != null)
        {
            Odgovor? odgovor;
            for (int i = 0; i < student.PostavljeniOdgovoriIds.Count(); i++)
            {
                odgovor = await _context.Odgovori.FindAsync(student.PostavljeniOdgovoriIds[i]);
                if (odgovor != null)
                {
                    if (!studentZaAzuriranje.PostavljeniOdgovori!.Contains(odgovor))
                        studentZaAzuriranje.PostavljeniOdgovori.Add(odgovor);
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(studentZaAzuriranje);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ValidateDbSetFilter<Student>))]
    [TypeFilter(typeof(ValidateIdFilter<Student>))]
    public async Task<ActionResult<string>> ObrisiStudenta(int id)
    {
        // iz ValidateIdFilter-a
        var studentZaBrisanje = HttpContext.Items["entity"] as Student;
        _context.Studenti.Remove(studentZaBrisanje!);
        await _context.SaveChangesAsync();

        return Ok("Student uspešno obrisan");
    }
}