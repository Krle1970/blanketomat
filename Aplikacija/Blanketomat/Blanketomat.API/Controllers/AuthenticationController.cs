using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Filters.AuthenticationFilters;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly BlanketomatContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationController(BlanketomatContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register-profesor")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult<string>> RegisterProfesor([FromBody] RegisterProfesorRequestDTO user)
    {
        PasswordManager.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
        Profesor profesor = new Profesor
        {
            Ime = user.Ime,
            Prezime = user.Prezime,
            Email = user.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Katedra = user.Katedra,
            Smerovi = user.Smerovi,
            Predmeti = user.Predmeti
        };

        _context.Profesori.Add(profesor);
        await _context.SaveChangesAsync();

        return Ok("Profesor uspešno registrovan");
    }

    [HttpPost("login-profesor")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult<ProfesorLoginResponseDTO>> LoginProfesor([FromBody] LoginRequestDTO user)
    {
        var profesor = await _context.Profesori.FirstOrDefaultAsync(x => x.Email == user.Email);
        if (profesor == null)
        {
            return Unauthorized("Profesor sa ovim podacima ne postoji");
        }
        if (!PasswordManager.VerifyPasswordHash(user.Password, profesor.PasswordHash, profesor.PasswordSalt))
        {
            return Unauthorized("Profesor sa ovim podacima ne postoji");
        }

        string token = TokenManager.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value!);
        profesor.Token = token;
        await _context.SaveChangesAsync();

        return Ok(profesor);
    }
}