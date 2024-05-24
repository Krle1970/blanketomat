using Blanketomat.API.Context;
using Blanketomat.API.DTOs;
using Blanketomat.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Web;

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
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
        {
            context.ModelState.AddModelError("Authorization", "Authorization header nije naveden");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status401Unauthorized
            };
            context.Result = new NotFoundObjectResult(problemDetails);
        }

        if (context.HttpContext.User.Identity.IsAuthenticated)
        {

        }

        //try
        //{
        //    var user = context.ActionArguments["user"] as LoginDTO;

        //    if (!user!.Email.Contains("elfak.rs") && !user.Email.Contains("elfak.ni.ac.rs"))
        //    {
        //        context.ModelState.AddModelError("Authentication", "Nevalidan email. Domain mora biti elfak.rs ili elfak.ni.ac.rs");
        //        var problemDetails = new ValidationProblemDetails(context.ModelState)
        //        {
        //            Status = StatusCodes.Status400BadRequest
        //        };
        //        context.Result = new BadRequestObjectResult(problemDetails);
        //    }
        //    else if (user.AccountType!.ToLower() == "administrator")
        //    {
        //        var administrator = _context.Administratori.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        //        if (administrator == null)
        //        {
        //            context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Administrator sa ovim podacima ne postoji.");
        //            var problemDetails = new ValidationProblemDetails(context.ModelState)
        //            {
        //                Status = StatusCodes.Status404NotFound
        //            };
        //            context.Result = new NotFoundObjectResult(problemDetails);
        //        }
        //        else
        //        {
        //            user.FullName = administrator.Ime;
        //            context.HttpContext.Items["user"] = user;
        //        }
        //    }
        //    else if (user.AccountType.ToLower() == "student")
        //    {
        //        var student = _context.Studenti.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        //        if (student == null)
        //        {
        //            context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Student sa ovim podacima ne postoji.");
        //            var problemDetails = new ValidationProblemDetails(context.ModelState)
        //            {
        //                Status = StatusCodes.Status404NotFound
        //            };
        //            context.Result = new NotFoundObjectResult(problemDetails);
        //        }
        //        else
        //        {
        //            user.FullName = student.Ime + " " + student.Prezime;
        //            context.HttpContext.Items["user"] = user;
        //        }
        //    }
        //    else if (user.AccountType.ToLower() == "profesor")
        //    {
        //        var profesor = _context.Profesori.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        //        if (profesor == null)
        //        {
        //            context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Profesor sa ovim podacima ne postoji.");
        //            var problemDetails = new ValidationProblemDetails(context.ModelState)
        //            {
        //                Status = StatusCodes.Status404NotFound
        //            };
        //            context.Result = new NotFoundObjectResult(problemDetails);
        //        }
        //        else
        //        {
        //            user.FullName = profesor.Ime + " " + profesor.Prezime;
        //            context.HttpContext.Items["user"] = user;
        //        }
        //    }
        //    else if (user.AccountType.ToLower() == "asistent")
        //    {
        //        var asistent = _context.Asistenti.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        //        if (asistent == null)
        //        {
        //            context.ModelState.AddModelError("Authentication", "Neispravni podaci ili Asistent sa ovim podacima ne postoji.");
        //            var problemDetails = new ValidationProblemDetails(context.ModelState)
        //            {
        //                Status = StatusCodes.Status404NotFound
        //            };
        //            context.Result = new NotFoundObjectResult(problemDetails);
        //        }
        //        else
        //        {
        //            user.FullName = asistent.Ime + " " + asistent.Prezime;
        //            context.HttpContext.Items["user"] = user;
        //        }
        //    }
        //    else
        //    {
        //        context.ModelState.AddModelError("Authentication", "Neispravan tip naloga. Tip naloga mora biti administrator, student, profesor ili asistent.");
        //        var problemDetails = new ValidationProblemDetails(context.ModelState)
        //        {
        //            Status = StatusCodes.Status400BadRequest
        //        };
        //        context.Result = new BadRequestObjectResult(problemDetails);
        //    }
        //}
        //catch (KeyNotFoundException)
        //{
        //    context.ModelState.AddModelError("Authentication", "Korisnik mora biti naveden.");
        //    var problemDetails = new ValidationProblemDetails(context.ModelState)
        //    {
        //        Status = StatusCodes.Status400BadRequest
        //    };
        //    context.Result = new BadRequestObjectResult(problemDetails);
        //}
    }
}