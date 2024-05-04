using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajPredmetFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajPredmetFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //base.OnActionExecuting(context);
        var predmet = context.ActionArguments["predmet"] as Predmet;
        if (predmet == null)
        {
            context.ModelState.AddModelError("Predmet", "Predmet objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var studentId = predmet.Id;
            if (studentId <= 0)
            {
                context.ModelState.AddModelError("Predmet", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                if (_context.Predmeti == null)
                {
                    context.ModelState.AddModelError("Predmet", "Tabela Predmeti ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    var predmetZaAzuriranje = _context.Predmeti.Find(studentId);
                    if (predmetZaAzuriranje == null)
                    {
                        context.ModelState.AddModelError("Predmet", "Predmet ne postoji u bazi podataka.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["predmet"] = predmetZaAzuriranje;
                    }
                }
            }
        }
    }
}