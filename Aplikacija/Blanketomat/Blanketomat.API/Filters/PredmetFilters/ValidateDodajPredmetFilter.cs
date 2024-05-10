using Blanketomat.API.Context;
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
        var predmet = context.ActionArguments["predmet"] as Predmet;
        if (predmet == null)
        {
            context.ModelState.AddModelError("Predmet", "Predmet objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Predmeti == null)
            {
                context.ModelState.AddModelError("Predmet", "Tabela Predmeti ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojeciPredmet = _context.Predmeti.FirstOrDefault(x =>
                    predmet.Akreditacija != null && x.Akreditacija != null &&
                    !string.IsNullOrWhiteSpace(predmet.Akreditacija.Naziv) &&
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
    }
}