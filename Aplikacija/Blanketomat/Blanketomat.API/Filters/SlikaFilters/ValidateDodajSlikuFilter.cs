using Blanketomat.API.Context;
using Blanketomat.API.DTOs.SlikaDTOs;
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
        var slika = context.ActionArguments["novaSlika"] as DodajSlikuDTO;
        var postojecaSlika = _context.Slike.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(slika!.Putanja) &&
            !string.IsNullOrWhiteSpace(x.Putanja) &&
            slika.Putanja.ToLower() == x.Putanja.ToLower()
            );

        if (postojecaSlika != null)
        {
            context.ModelState.AddModelError("Slika", "Slika vec postoji");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}