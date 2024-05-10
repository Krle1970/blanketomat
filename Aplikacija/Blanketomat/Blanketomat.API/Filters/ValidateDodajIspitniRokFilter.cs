using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajIspitniRokFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajIspitniRokFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var ispitniRok = context.ActionArguments["ispitniRok"] as IspitniRok;
        if (ispitniRok == null)
        {
            context.ModelState.AddModelError("IspitniRok", "IspitniRok objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.IspitniRokovi == null)
            {
                context.ModelState.AddModelError("IspitniRok", "Tabela IspitniRokovi ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojeciIspitniRok = _context.IspitniRokovi.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(ispitniRok.Naziv) &&
                    !string.IsNullOrWhiteSpace(x.Naziv) &&
                    ispitniRok.Naziv.ToLower() == x.Naziv.ToLower()
                    );

                if (postojeciIspitniRok != null)
                {
                    context.ModelState.AddModelError("IspitniRok", "Ispitni rok vec postoji.");
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