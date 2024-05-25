using Blanketomat.API.Context;
using Blanketomat.API.DTOs.PonavljanjeIspitnogRokaDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.PonavljanjeIspitnogRokaFilters;

public class ValidateDodajPonavljanjeIspitnogRokaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajPonavljanjeIspitnogRokaFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var ponavljanjeIspitnogRoka = context.ActionArguments["novoPonavljanjeIspitnogRoka"] as DodajPonavljanjeIspitnogRokaDTO;
        var postojecePonavljanjeIspitnogRoka = _context.PonavljanjaIspitnihRokova.FirstOrDefault(x =>
            ponavljanjeIspitnogRoka!.Datum == x.Datum
            );

        if (postojecePonavljanjeIspitnogRoka != null)
        {
            context.ModelState.AddModelError("PonavljanjeIspitnogRoka", "Ponavljanje ispitnog roka vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}