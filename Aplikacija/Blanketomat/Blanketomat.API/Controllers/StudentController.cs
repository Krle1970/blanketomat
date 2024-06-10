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
    public async Task<ActionResult<Student>> AzurirajStudenta(int id, [FromBody] StudentDTO studentDTO)
    {
        var studentZaAzuriranje = await _context.Studenti.FindAsync(id);

        if (studentZaAzuriranje == null)
        {
            return NotFound();
        }

        PasswordManager.CreatePasswordHash(studentDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

        studentZaAzuriranje.Ime = studentDTO.Ime;
        studentZaAzuriranje.Prezime = studentDTO.Prezime;
        studentZaAzuriranje.Email = studentDTO.Email;
        studentZaAzuriranje.PasswordHash = passwordHash;
        studentZaAzuriranje.PasswordSalt = passwordSalt;

        if (studentDTO.Akreditacija != null)
        {
            studentZaAzuriranje.Akreditacija = studentDTO.Akreditacija;
        }
        if (studentDTO.Smer != null)
        {
            studentZaAzuriranje.Smer = studentDTO.Smer;
        }
        if (studentDTO.Predmeti != null)
        {
            studentZaAzuriranje.Predmeti = studentDTO.Predmeti;
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok(studentZaAzuriranje);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
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