using Blanketomat.API.Context;
using Blanketomat.API.Models;
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
        var ponavljanjeIspitnogRoka = context.ActionArguments["ponavljanjeIspitnogRoka"] as PonavljanjeIspitnogRoka;
        if (ponavljanjeIspitnogRoka == null)
        {
            context.ModelState.AddModelError("PonavljanjeIspitnogRoka", "PonavljanjeIspitnogRoka objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.PonavljanjaIspitnihRokova == null)
            {
                context.ModelState.AddModelError("PonavljanjeIspitnogRoka", "Tabela PonavljanjaIspitnihRokova ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            //else
            //{
            //    var postojecePonavljanjeIspitnogRoka = _context.PonavljanjaIspitnihRokova.FirstOrDefault(x =>
            //        !string.IsNullOrWhiteSpace(ponavljanjeIspitnogRoka.Datum) &&
            //        !string.IsNullOrWhiteSpace(x.Datum) &&
            //        ponavljanjeIspitnogRoka.Datum.ToLower() == x.Datum.ToLower()
            //        );

            //    if (postojecePonavljanjeIspitnogRoka != null)
            //    {
            //        context.ModelState.AddModelError("PonavljanjeIspitnogRoka", "Ponavljanje ispitnog roka vec postoji.");
            //        var problemDetails = new ValidationProblemDetails(context.ModelState)
            //        {
            //            Status = StatusCodes.Status400BadRequest
            //        };
            //        context.Result = new BadRequestObjectResult(problemDetails);
            //    }
            //}
        }
    }
}