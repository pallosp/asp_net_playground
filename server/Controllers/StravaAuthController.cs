namespace server.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using server.Models;
using server.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class StravaAuthController : ControllerBase
{
  private readonly HttpClient _http;
  private readonly IConfiguration _config;

  public StravaAuthController(HttpClient http, IConfiguration config)
  {
    _http = http;
    _config = config;
  }

  [HttpGet("login")]
  public IActionResult Login()
  {
    var clientId = _config["Strava:ClientId"];
    var redirectUri = "http://localhost:5252/api/v1/stravaauth/callback";
    var url = $"https://www.strava.com/oauth/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUri}&scope=activity:read_all";
    return Redirect(url);
  }

  [HttpGet("callback")]
  public async Task<IActionResult> Callback([FromQuery] string code)
  {
    var clientId = _config["Strava:ClientId"];
    var clientSecret = _config["Strava:ClientSecret"];

    var tokenRequestParams = new Dictionary<string, string>
    {
      ["client_id"] = clientId!,
      ["client_secret"] = clientSecret!,
      ["code"] = code,
      ["grant_type"] = "authorization_code"
    };

    var response = await _http.PostAsync(
        "https://www.strava.com/oauth/token",
        new FormUrlEncodedContent(tokenRequestParams));

    var json = await response.Content.ReadAsStringAsync();
    var token = JsonSerializer.Deserialize<StravaAuthResponse>(json);

    if (token != null)
    {
      HttpContext.Session.SetString("StravaToken", JsonSerializer.Serialize(token));
      return Redirect("/");
    }

    return BadRequest("Failed to get token");
  }

  [HttpGet("me")]
  public IActionResult Me()
  {
    var tokenJson = HttpContext.Session.GetString("StravaToken");
    if (string.IsNullOrEmpty(tokenJson)) return Unauthorized();

    var token = JsonSerializer.Deserialize<StravaAuthResponse>(tokenJson);
    if (token == null) return Unauthorized();

    var dto = DtoMapper.ToDto(token.Athlete);
    return Ok(dto);
  }

  [HttpPost("disconnect")]
  public IActionResult Disconnect()
  {
    HttpContext.Session.Remove("StravaToken");
    return Ok();
  }
}
