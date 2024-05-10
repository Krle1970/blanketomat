using Blanketomat.API.Context;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Filters.AuthenticationFilters;

public class ValidateAuthenticationLoginFilter : ActionFilterAttribute
{
    private readonly BlanketomatContext _context;

    public ValidateAuthenticationLoginFilter(BlanketomatContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var email = context.ActionArguments["email"] as string;
        var password = context.ActionArguments["password"] as string;
        var accountType = context.ActionArguments["accountType"] as string;

        if (string.IsNullOrWhiteSpace(email))
        {
            context.ModelState.AddModelError("Authentication", "Email mora biti naveden.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else if (string.IsNullOrWhiteSpace(password))
        {
            context.ModelState.AddModelError("Authentication", "Password mora biti naveden.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else if (string.IsNullOrWhiteSpace(accountType))
        {
            context.ModelState.AddModelError("Authentication", "Tip naloga mora biti naveden.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            if (!email.Contains("elfak.rs") && !email.Contains("elfak.ni.ac.rs"))
            {
                context.ModelState.AddModelError("Authentication", "Nevalidan email. Domain mora biti elfak.rs ili elfak.ni.ac.rs");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else if (accountType.ToLower() == "administrator")
            {
                var administrator = _context.Administratori.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (administrator == null)
                {
                    context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Administrator sa ovim podacima ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
            else if (accountType.ToLower() == "student")
            {
                var student = _context.Studenti.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (student == null)
                {
                    context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Student sa ovim podacima ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
            else if (accountType.ToLower() == "profesor")
            {
                var profesor = _context.Profesori.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (profesor == null)
                {
                    context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Profesor sa ovim podacima ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
            else if (accountType.ToLower() == "asistent")
            {
                var asistent = _context.Asistenti.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (asistent == null)
                {
                    context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Asistent sa ovim podacima ne postoji.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
            else if (accountType.ToLower() != "administrator" && accountType.ToLower() != "student" &&
                     accountType.ToLower() != "profesor" && accountType.ToLower() != "asistent")
            {
                context.ModelState.AddModelError("Authentication", "Neispravan tip naloga. Tip naloga mora biti administrator, student, profesor ili asistent.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}