using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajPonavljanjeIspitnogRoka : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajPonavljanjeIspitnogRoka(BlanketomatContext context)
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
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var ponavljanjeIspitnogRokaId = ponavljanjeIspitnogRoka.Id;
            if (ponavljanjeIspitnogRokaId <= 0)
            {
                context.ModelState.AddModelError("PonavljanjeIspitnogRoka", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
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
                else
                {
                    var ponavljanjeIspitnogRokaZaAzuriranje = _context.Zadaci.Find(ponavljanjeIspitnogRokaId);
                    if (ponavljanjeIspitnogRokaZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("PonavljanjeIspitnogRoka", "Ponavljanje ispitnog roka ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["ponavljanjeIspitnogRoka"] = ponavljanjeIspitnogRokaZaAzuriranje;
                    }
                }
            }
        }
    }
}