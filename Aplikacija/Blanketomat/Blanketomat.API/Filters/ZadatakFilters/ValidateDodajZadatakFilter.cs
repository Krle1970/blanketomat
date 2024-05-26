using Blanketomat.API.Context;
using Blanketomat.API.DTOs.ZadatakDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.ZadatakFilters;

public class ValidateDodajZadatakFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajZadatakFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var zadatak = context.ActionArguments["noviZadatak"] as ZadatakDTO;
        var postojeciZadatak = _context.Zadaci.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(zadatak!.Tekst) &&
            !string.IsNullOrWhiteSpace(x.Tekst) &&
            zadatak.Tekst.ToLower() == x.Tekst.ToLower()
            );

        if (postojeciZadatak != null)
        {
            context.ModelState.AddModelError("Zadatak", "Zadatak vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}