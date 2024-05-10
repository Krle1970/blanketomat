using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajAdministratoraFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajAdministratoraFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var administrator = context.ActionArguments["administrator"] as Administrator;
        if (administrator == null)
        {
            context.ModelState.AddModelError("Administrator", "Administrator objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var administratorId = administrator.Id;
            if (administratorId <= 0)
            {
                context.ModelState.AddModelError("Administrator", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Administratori == null)
                {
                    context.ModelState.AddModelError("Administrator", "Tabela Administratori ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var administratorZaAzuriranje = _context.Administratori.Find(administratorId);
                    if (administratorZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Administrator", "Administrator ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["administrator"] = administratorZaAzuriranje;
                    }
                }
            }
        }
    }
}