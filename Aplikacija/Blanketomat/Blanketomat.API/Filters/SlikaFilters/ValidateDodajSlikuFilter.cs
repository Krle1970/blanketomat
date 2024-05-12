using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.SlikaFilters;

public class ValidateDodajSlikuFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajSlikuFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var slika = context.ActionArguments["slika"] as Slika;
        if (slika == null)
        {
            context.ModelState.AddModelError("Slika", "Slika objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Slike == null)
            {
                context.ModelState.AddModelError("Slika", "Tabela Slike ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var postojecaSlika = _context.Slike.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(slika.Putanja) &&
                    !string.IsNullOrWhiteSpace(x.Putanja) &&
                    slika.Putanja.ToLower() == x.Putanja.ToLower()
                    );

                if (postojecaSlika != null)
                {
                    context.ModelState.AddModelError("Slika", "Slika vec postoji.");
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