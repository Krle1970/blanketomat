using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajKomentaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajKomentaFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var komentar = context.ActionArguments["komentar"] as Komentar;
        if (komentar == null)
        {
            context.ModelState.AddModelError("Komentar", "Komentar objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (_context.Komentari == null)
            {
                context.ModelState.AddModelError("Komentar", "Tabela Komentari ne postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}