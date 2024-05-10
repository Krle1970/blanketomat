using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajAkreditacijuFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajAkreditacijuFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var akreditacija = context.ActionArguments["akreditacija"] as Akreditacija;
        if (akreditacija == null)
        {
            context.ModelState.AddModelError("Akreditacija", "Akreditacija objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Akreditacije == null)
            {
                context.ModelState.AddModelError("Akreditacija", "Tabela Akreditacije ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojecaAkreditacija = _context.Akreditacije.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(akreditacija.Naziv) &&
                    !string.IsNullOrWhiteSpace(x.Naziv) &&
                    akreditacija.Naziv.ToLower() == x.Naziv.ToLower()
                    );

                if (postojecaAkreditacija != null)
                {
                    context.ModelState.AddModelError("Akreditacija", "Akreditacija vec postoji.");
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