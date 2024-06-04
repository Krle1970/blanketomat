using Blanketomat.API.Context;
using Blanketomat.API.DTOs.AdministratorDTOs;
using Blanketomat.API.DTOs.AkreditacijaDTOs;
using Blanketomat.API.DTOs.KatedraDTOs;
using Blanketomat.API.DTOs.KomentarDTOs;
using Blanketomat.API.DTOs.LoginDTOs;
using Blanketomat.API.DTOs.OdgovorDTOs;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.DTOs.RegisterDTOs;
using Blanketomat.API.DTOs.SmerDTOs;
using Blanketomat.API.Filters.AuthenticationFilters;
using Blanketomat.API.Helper;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BlanketomatContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BlanketomatContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register-profesor"), Authorize(Roles = "Administrator")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult> RegisterProfesor([FromBody] RegisterProfesorRequestDTO user)
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

        return Created();
        //return Ok("Profesor uspešno registrovan");
    }

    [HttpPost("register-asistent"), Authorize(Roles = "Administrator")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult> RegisterAsistent([FromBody] RegisterAsistentRequestDTO user)
    {
        PasswordManager.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
        Asistent asistent = new Asistent
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

        _context.Asistenti.Add(asistent);
        await _context.SaveChangesAsync();

        return Created();
        //return Ok("Asistent uspešno registrovan");
    }

    [HttpPost("register-student"), Authorize(Roles = "Administrator")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult> RegisterStudent([FromBody] RegisterStudentRequestDTO user)
    {
        PasswordManager.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
        Student student = new Student
        {
            Ime = user.Ime,
            Prezime = user.Prezime,
            Email = user.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Akreditacija = user.Akreditacija,
            Smer = user.Smer,
            Predmeti = user.Predmeti
        };

        _context.Studenti.Add(student);
        await _context.SaveChangesAsync();

        return Created();
        //return Ok("Student uspešno registrovan");
    }

    [HttpPost("login-admin")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult<AdministatorLoginResponseDTO>> LoginAdministrator([FromBody] LoginRequestDTO user)
    {
        var administrator = await _context.Administratori.FirstOrDefaultAsync(x => x.Email == user.Email);
        if (administrator == null)
        {
            return Unauthorized("Administrator sa ovim podacima ne postoji");
        }
        if (!PasswordManager.VerifyPasswordHash(user.Password, administrator.PasswordHash, administrator.PasswordSalt))
        {
            return Unauthorized("Administrator sa ovim podacima ne postoji");
        }

        string token = TokenManager.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value!);
        administrator.Token = token;

        AdministatorLoginResponseDTO respose = new AdministatorLoginResponseDTO
        {
            Ime = administrator.Ime,
            Prezime = administrator.Prezime,
            Email = administrator.Email,
            Password = user.Password,
            Token = token
        };

        await _context.SaveChangesAsync();

        return Ok(respose);
    }

    [HttpPost("login-student")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult<StudentLoginResponseDTO>> LoginStudent([FromBody] LoginRequestDTO user)
    {
        var student = await _context.Studenti.FirstOrDefaultAsync(x => x.Email == user.Email);
        if (student == null)
        {
            return Unauthorized("Student sa ovim podacima ne postoji");
        }
        if (!PasswordManager.VerifyPasswordHash(user.Password, student.PasswordHash, student.PasswordSalt))
        {
            return Unauthorized("Student sa ovim podacima ne postoji");
        }

        AkreditacijaBasicDTO? akreditacija = null;

        if (student.Akreditacija != null)
        {
            akreditacija = new AkreditacijaBasicDTO { Id = student.Akreditacija.Id, Naziv = student.Akreditacija.Naziv };
        }

        SmerBasicDTO? smer = null;

        if (student.Smer != null)
        {
            smer = new SmerBasicDTO { Id = student.Smer.Id, Naziv = student.Smer.Naziv };
        }

        List<PredmetBasicDTO> predmeti = new List<PredmetBasicDTO>();

        if (student.Predmeti != null)
        {
            PredmetBasicDTO predmetBasic;
            foreach (var predmet in student.Predmeti)
            {
                predmetBasic = new PredmetBasicDTO { Id = predmet.Id, Naziv = predmet.Naziv, Godina = predmet.Godina };
                predmeti.Add(predmetBasic);
            }
        }

        List<KomentarBasicDTO> komentari = new List<KomentarBasicDTO>();

        if (student.PostavljeniKomentari != null)
        {
            KomentarBasicDTO komentarBasic;
            foreach (var komentar in student.PostavljeniKomentari)
            {
                komentarBasic = new KomentarBasicDTO { Id = komentar.Id, Tekst = komentar.Tekst, Lajkovi = komentar.Lajkovi };
                komentari.Add(komentarBasic);
            }
        }

        List<OdgovorBasicDTO> odgovori = new List<OdgovorBasicDTO>();

        if (student.PostavljeniOdgovori != null)
        {
            OdgovorBasicDTO odgovorBasic;
            foreach (var odgovor in student.PostavljeniOdgovori)
            {
                odgovorBasic = new OdgovorBasicDTO { Id = odgovor.Id, Tekst = odgovor.Tekst, Lajkovi = odgovor.Lajkovi };
                odgovori.Add(odgovorBasic);
            }
        }

        string token = TokenManager.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value!);
        student.Token = token;

        StudentLoginResponseDTO response = new StudentLoginResponseDTO
        {
            Ime = student.Ime,
            Prezime = student.Prezime,
            Email = student.Email,
            Password = user.Password,
            Token = student.Token,
            Akreditacija = akreditacija,
            Smer = smer,
            Predmeti = predmeti,
            Komentari = komentari,
            Odgovori = odgovori
        };

        await _context.SaveChangesAsync();

        return Ok(response);
    }

    [HttpPost("login-asistent")]
    //[TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public async Task<ActionResult<AsistentLoginResponseDTO>> LoginAsistent([FromBody] LoginRequestDTO user)
    {
        var asistent = await _context.Asistenti.FirstOrDefaultAsync(x => x.Email == user.Email);
        if (asistent == null)
        {
            return Unauthorized("Asistent sa ovim podacima ne postoji");
        }
        if (!PasswordManager.VerifyPasswordHash(user.Password, asistent.PasswordHash, asistent.PasswordSalt))
        {
            return Unauthorized("Asistent sa ovim podacima ne postoji");
        }

        List<SmerBasicDTO> smerovi = new List<SmerBasicDTO>();

        if (asistent.Smerovi != null)
        {
            SmerBasicDTO smerBasic;
            foreach (var smer in asistent.Smerovi)
            {
                smerBasic = new SmerBasicDTO { Id = smer.Id, Naziv = smer.Naziv };
                smerovi.Add(smerBasic);
            }
        }

        List<PredmetBasicDTO> predmeti = new List<PredmetBasicDTO>();

        if (asistent.Predmeti != null)
        {
            PredmetBasicDTO predmetBasic;
            foreach (var predmet in asistent.Predmeti)
            {
                predmetBasic = new PredmetBasicDTO { Id = predmet.Id, Naziv = predmet.Naziv, Godina = predmet.Godina };
                predmeti.Add(predmetBasic);
            }
        }

        KatedraBasicDTO? katedra = null;

        if (asistent.Katedra != null)
        {
            katedra = new KatedraBasicDTO { Id = asistent.Katedra.Id, Naziv = asistent.Katedra.Naziv };
        }

        List<KomentarBasicDTO> komentari = new List<KomentarBasicDTO>();

        if (asistent.LajkovaniKomentari != null)
        {
            KomentarBasicDTO komentarBasic;
            foreach (var komentar in asistent.LajkovaniKomentari)
            {
                komentarBasic = new KomentarBasicDTO { Id = komentar.Id, Tekst = komentar.Tekst, Lajkovi = komentar.Lajkovi };
                komentari.Add(komentarBasic);
            }
        }

        List<OdgovorBasicDTO> odgovori = new List<OdgovorBasicDTO>();

        if (asistent.LajkovaniOdgovori != null)
        {
            OdgovorBasicDTO odgovorBasic;
            foreach (var odgovor in asistent.LajkovaniOdgovori)
            {
                odgovorBasic = new OdgovorBasicDTO { Id = odgovor.Id, Tekst = odgovor.Tekst, Lajkovi = odgovor.Lajkovi };
                odgovori.Add(odgovorBasic);
            }
        }

        string token = TokenManager.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value!);
        asistent.Token = token;

        AsistentLoginResponseDTO response = new AsistentLoginResponseDTO
        {
            Ime = asistent.Ime,
            Prezime = asistent.Prezime,
            Email = asistent.Email,
            Password = user.Password,
            Token = asistent.Token,
            Smerovi = smerovi,
            Predmeti = predmeti,
            Katedra = katedra,
            LajkovaniKomentari = komentari,
            LajkovaniOdgovori = odgovori
        };

        await _context.SaveChangesAsync();

        return Ok(response);
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

        List<SmerBasicDTO> smerovi = new List<SmerBasicDTO>();

        if (profesor.Smerovi != null) 
        {
            SmerBasicDTO smerBasic;
            foreach (var smer in profesor.Smerovi)
            {
                smerBasic = new SmerBasicDTO { Id = smer.Id, Naziv = smer.Naziv };
                smerovi.Add(smerBasic);
            }
        }

        List<PredmetBasicDTO> predmeti = new List<PredmetBasicDTO>();

        if (profesor.Predmeti != null)
        {
            PredmetBasicDTO predmetBasic;
            foreach (var predmet in profesor.Predmeti)
            {
                predmetBasic = new PredmetBasicDTO { Id = predmet.Id, Naziv = predmet.Naziv, Godina = predmet.Godina };
                predmeti.Add(predmetBasic);
            }
        }

        KatedraBasicDTO? katedra = null;

        if (profesor.Katedra != null)
        {
            katedra = new KatedraBasicDTO { Id = profesor.Katedra.Id, Naziv = profesor.Katedra.Naziv };
        }

        List<KomentarBasicDTO> komentari = new List<KomentarBasicDTO>();

        if (profesor.LajkovaniKomentari != null)
        {
            KomentarBasicDTO komentarBasic;
            foreach (var komentar in profesor.LajkovaniKomentari)
            {
                komentarBasic = new KomentarBasicDTO { Id = komentar.Id, Tekst = komentar.Tekst, Lajkovi = komentar.Lajkovi };
                komentari.Add(komentarBasic);
            }
        }

        List<OdgovorBasicDTO> odgovori = new List<OdgovorBasicDTO>();

        if (profesor.LajkovaniOdgovori != null)
        {
            OdgovorBasicDTO odgovorBasic;
            foreach (var odgovor in profesor.LajkovaniOdgovori)
            {
                odgovorBasic = new OdgovorBasicDTO { Id = odgovor.Id, Tekst = odgovor.Tekst, Lajkovi = odgovor.Lajkovi };
                odgovori.Add(odgovorBasic);
            }
        }

        string token = TokenManager.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value!);
        profesor.Token = token;

        ProfesorLoginResponseDTO response = new ProfesorLoginResponseDTO
        {
            Ime = profesor.Ime,
            Prezime = profesor.Prezime,
            Email = profesor.Email,
            Password = user.Password,
            Token = profesor.Token,
            Smerovi = smerovi,
            Predmeti = predmeti,
            Katedra = katedra,
            LajkovaniKomentari = komentari,
            LajkovaniOdgovori = odgovori
        };
        
        await _context.SaveChangesAsync();

        return Ok(response);
    }
}