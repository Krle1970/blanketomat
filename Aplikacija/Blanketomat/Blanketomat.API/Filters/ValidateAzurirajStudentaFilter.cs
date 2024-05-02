using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blanketomat.API.Filters;

public class ValidateAzurirajStudentaFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAzurirajStudentaFilter(BlanketomatContext context)
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
            context.Result = new NotFoundObjectResult(problemDetails);
        }
        else
        {
            var studentId = student.Id;
            if (studentId <= 0)
            {
                context.ModelState.AddModelError("Student", "Nevalidan Id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            else
            {
                var studentZaAzuriranje = _context.Studenti.Find(studentId);
                if (studentZaAzuriranje == null)
                {
                    context.ModelState.AddModelError("Student", "Student ne postoji u bazi podataka.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    context.HttpContext.Items["student"] = studentZaAzuriranje;
                }
            }
        }
    }
}