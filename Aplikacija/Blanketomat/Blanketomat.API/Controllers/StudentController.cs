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
    public async Task<ActionResult<PagingResponseDTO<Student>>> VratiStudente(int page, int count)
    {
        var brojRezultata = count;
        var brojStranica = Math.Ceiling(_context.Studenti.Count() / (float)brojRezultata);

        var studenti = await _context.Studenti
            .Skip((page - 1) * brojRezultata)
            .Take(brojRezultata)
            .ToListAsync();

        var response = new PagingResponseDTO<Student>
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
        studentZaAzuriranje.PostavljeniKomentari = student.PostavljeniKomentari;
        studentZaAzuriranje.PostavljeniOdgovori = student.PostavljeniOdgovori;

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