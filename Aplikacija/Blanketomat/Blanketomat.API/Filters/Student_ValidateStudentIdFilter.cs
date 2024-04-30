using Blanketomat.API.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class Student_ValidateStudentIdFilter<T> : ActionFilterAttribute where T : class
{
    private readonly BlanketomatContext _context;

    public Student_ValidateStudentIdFilter(BlanketomatContext context)
    {
        _context = context;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //base.OnActionExecuting(context);

        var Id = context.ActionArguments["id"] as int?;
        if (Id.HasValue)
        {
            if (Id.Value <= 0)
            {
                context.ModelState.AddModelError($"{typeof(T).Name}" + "Id", $"{typeof(T).Name}" + "Id" + " je nevalidan.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var osoba = _context.Set<T>().Find(Id.Value);

                if (osoba == null)
                {
                    context.ModelState.AddModelError($"{typeof(T).Name}" + "Id", $"{typeof(T).Name}" + "Id" + " ne postoji u bazi podataka.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}