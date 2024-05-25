using Blanketomat.API.DTOs.KomentarDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.KomentarFilters;

public class ValidateDodajKomentaFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var komentar = context.ActionArguments["noviKomentar"] as DodajKomentarDTO;
        if (komentar == null)
        {
            context.ModelState.AddModelError("Komentar", "Komentar objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}