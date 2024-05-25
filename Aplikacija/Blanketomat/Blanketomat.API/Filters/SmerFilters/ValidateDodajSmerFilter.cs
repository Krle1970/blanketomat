using Blanketomat.API.Context;
using Blanketomat.API.DTOs.SmerDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.SmerFilters;

public class ValidateDodajSmerFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajSmerFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var smer = context.ActionArguments["noviSmer"] as DodajSmerDTO;
        var postojeciSmer = _context.Smerovi.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(smer!.Naziv) &&
            !string.IsNullOrWhiteSpace(x.Naziv) &&
            smer.Naziv.ToLower() == x.Naziv.ToLower()
            );

        if (postojeciSmer != null)
        {
            context.ModelState.AddModelError("Smer", "Smer vec postoji.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}