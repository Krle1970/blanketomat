using Blanketomat.API.Context;
using Blanketomat.API.Filters.AuthenticationFilters;
using Microsoft.AspNetCore.Mvc;

namespace Blanketomat.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly BlanketomatContext _context;

    public AuthenticationController(BlanketomatContext context)
    {
        _context = context;
    }

    [HttpPost]
    [TypeFilter(typeof(ValidateAuthenticationLoginFilter))]
    public ActionResult Login(string email, string password, string accountType)
    {
        return NoContent();
    }
}