using Blanketomat.API.Context;
using Blanketomat.API.DTOs.PitanjeDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.PitanjeFilters;

public class ValidateDodajPitanjeFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajPitanjeFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var pitanje = context.ActionArguments["novoPitanje"] as DodajPitanjeDTO;
        var postojecePitanje = _context.Pitanja.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(pitanje!.Tekst) &&
            !string.IsNullOrWhiteSpace(x.Tekst) &&
            pitanje.Tekst.ToLower() == x.Tekst.ToLower()
            );

        if (postojecePitanje != null)
        {
            context.ModelState.AddModelError("Pitanje", "Pitanje vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}