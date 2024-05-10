using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.IspitniRokFIlters;

public class ValidateAzurirajIspitniRokFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajIspitniRokFilter(BlanketomatContext context)
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
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var ispitniRokId = ispitniRok.Id;
            if (ispitniRokId <= 0)
            {
                context.ModelState.AddModelError("IspitniRok", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
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
                    var ispitniRokZaAzuriranje = _context.IspitniRokovi.Find(ispitniRokId);
                    if (ispitniRokZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("IspitniRok", "Ispitni rok ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["ispitniRok"] = ispitniRokZaAzuriranje;
                    }
                }
            }
        }
    }
}