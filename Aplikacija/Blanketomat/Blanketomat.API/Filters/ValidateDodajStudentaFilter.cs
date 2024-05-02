using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateDodajStudentaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateDodajStudentaFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //base.OnActionExecuting(context);
        var student = context.ActionArguments["student"] as Student;
        if (student == null)
        {
            context.ModelState.AddModelError("Student", "Student objekat je null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            var postojeciStudent = _context.Studenti.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(student.Email) &&
                !string.IsNullOrWhiteSpace(x.Email) &&
                student.Email.ToLower() == x.Email.ToLower()
                );

            if (postojeciStudent != null)
            {
                context.ModelState.AddModelError("Student", "Student vec postoji.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}