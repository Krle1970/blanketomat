using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajSmerFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajSmerFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var smer = context.ActionArguments["smer"] as Smer;
        if (smer == null)
        {
            context.ModelState.AddModelError("Smer", "Smer objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Smerovi == null)
            {
                context.ModelState.AddModelError("Smer", "Tabela Smerovi ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojeciSmer = _context.Smerovi.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(smer.Naziv) &&
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
    }
}