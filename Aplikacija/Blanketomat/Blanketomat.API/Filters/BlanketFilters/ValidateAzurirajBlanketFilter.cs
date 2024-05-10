using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters.BlanketFilters;

public class ValidateAzurirajBlanketFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajBlanketFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var blanket = context.ActionArguments["blanket"] as Blanket;
        if (blanket == null)
        {
            context.ModelState.AddModelError("Blanket", "Blanket objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var blanketId = blanket.Id;
            if (blanketId <= 0)
            {
                context.ModelState.AddModelError("Blanket", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Blanketi == null)
                {
                    context.ModelState.AddModelError("Blanket", "Tabela Blanketi ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var blanketZaAzuriranje = _context.Blanketi.Find(blanketId);
                    if (blanketZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Blanket", "Blanket ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["blanket"] = blanketZaAzuriranje;
                    }
                }
            }
        }
    }
}