using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using server.Models;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
  private readonly ILogger<AuthController> _logger;

  public AuthController(ILogger<AuthController> logger)
  {
    _logger = logger;
  }

  // Demo login endpoint
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest login)
  {
    _logger.LogInformation("Login attempt for user: {Username}", login.Username);

    if (login.Username == "demo" && login.Password == "demo")
    {
      // claim = key-value pair, with keys like name, role, email, etc.
      var claims = new List<Claim> { new Claim(ClaimTypes.Name, login.Username) };

      // identity = claim[] + auth schemee.g. cookie, JWT or OAuth
      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      // principal = identity[]
      var principal = new ClaimsPrincipal(identity);

      // creates an auth cookie that encodes the principal
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

      return Ok(new { username = login.Username });
    }

    return Unauthorized();
  }

  [HttpPost("logout")]
  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Ok();
  }

  [HttpGet("me")]
  public IActionResult Me()
  {
    if (User.Identity?.IsAuthenticated ?? false)
      return Ok(new { username = User.Identity.Name });

    return Unauthorized();
  }
}
