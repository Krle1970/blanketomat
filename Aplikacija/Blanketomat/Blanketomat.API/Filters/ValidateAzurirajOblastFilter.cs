using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajOblastFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajOblastFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var oblast = context.ActionArguments["oblast"] as Oblast;
        if (oblast == null)
        {
            context.ModelState.AddModelError("Oblast", "Oblast objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var oblastId = oblast.Id;
            if (oblastId <= 0)
            {
                context.ModelState.AddModelError("Oblast", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Oblasti == null)
                {
                    context.ModelState.AddModelError("Oblast", "Tabela Oblasti ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var oblastZaAzuriranje = _context.Oblasti.Find(oblastId);
                    if (oblastZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Oblast", "Oblast ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["oblast"] = oblastZaAzuriranje;
                    }
                }
            }
        }
    }
}