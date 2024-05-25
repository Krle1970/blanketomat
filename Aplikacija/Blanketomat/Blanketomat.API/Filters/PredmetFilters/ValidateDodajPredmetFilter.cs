using Blanketomat.API.Context;
using Blanketomat.API.DTOs.PredmetDTOs;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.PredmetFilters;

public class ValidateDodajPredmetFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajPredmetFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var predmet = context.ActionArguments["noviPredmet"] as DodajPredmetDTO;
        var akreditacija = _context.Akreditacije.Find(predmet!.AkreditacijaId);
        var postojeciPredmet = _context.Predmeti.FirstOrDefault(x =>
            akreditacija != null && x.Akreditacija != null &&
            !string.IsNullOrWhiteSpace(akreditacija.Naziv) &&
            !string.IsNullOrWhiteSpace(x.Akreditacija.Naziv) &&
            !string.IsNullOrWhiteSpace(predmet.Naziv) &&
            !string.IsNullOrWhiteSpace(x.Naziv) &&
            predmet.Naziv.ToLower() == x.Naziv.ToLower()
            );

        if (postojeciPredmet != null)
        {
            context.ModelState.AddModelError("Predmet", "Predmet vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}