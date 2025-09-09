using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
  public async Task<IActionResult> Callback([FromQuery] string code, [FromServices] StravaAthleteCache store)
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
    Console.WriteLine(json);

    var token = JsonSerializer.Deserialize<StravaAuthResponse>(json);

    if (token != null)
    {
      store.Save(token);
      return Redirect($"/?athleteId={token.Athlete.Id}");
    }

    return BadRequest("Failed to get token");
  }

  [HttpGet("me/{athleteId}")]
  public IActionResult Me(long athleteId, [FromServices] StravaAthleteCache store)
  {
    var token = store.Get(athleteId);
    if (token == null) return NotFound();
    return Ok(token.Athlete);
  }

  [HttpPost("disconnect/{athleteId}")]
  public IActionResult Disconnect(long athleteId, [FromServices] StravaAthleteCache store)
  {
    store.Delete(athleteId);
    return Ok();
  }
}
