using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AkreditacijaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public AkreditacijaController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult VratiSveAkreditacije()
    {
        return Ok(_unitOfWork.AkreditacijaRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult VratiAkreditaciju(int id)
    {
        return Ok(_unitOfWork.AkreditacijaRepository.Get(id));
    }

    [HttpPost]
    public ActionResult DodajAkreditaciju([FromBody]Akreditacija akreditacija)
    {
        _unitOfWork.AkreditacijaRepository.Add(akreditacija);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(VratiAkreditaciju), new { id =  akreditacija.Id }, akreditacija);
    }

    [HttpPut("{id}")]
    public ActionResult AzurirajAkreditaciju(int id, [FromBody] Akreditacija akreditacija)
    {
        var akreditacijaZaUpdate = _unitOfWork.AkreditacijaRepository.Get(id);
        akreditacijaZaUpdate.Naziv = akreditacija.Naziv;
        akreditacijaZaUpdate.Predmeti = akreditacija.Predmeti;
        akreditacijaZaUpdate.Studenti = akreditacija.Studenti;

        _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult ObrisiAkreditaciju(int id)
    {
        var akreditacijaZaBrisanje = _unitOfWork.AkreditacijaRepository.Get(id);
        _unitOfWork.AkreditacijaRepository.Remove(akreditacijaZaBrisanje);
        _unitOfWork.Save();

        return Ok(akreditacijaZaBrisanje);
    }
}