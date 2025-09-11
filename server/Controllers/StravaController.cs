namespace server.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using server.Models;
using server.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class StravaController : ControllerBase
{
  private readonly HttpClient _http;
  private readonly IConfiguration _config;

  public StravaController(HttpClient http, IConfiguration config)
  {
    _http = http;
    _config = config;
  }

  [HttpGet("login")]
  public IActionResult Login()
  {
    var clientId = _config["Strava:ClientId"];
    var redirectUrl = Url.Action(
        "Callback",
        ControllerContext.ActionDescriptor.ControllerName,
        null,
        Request.Scheme
    );
    var url = $"https://www.strava.com/oauth/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUrl}&scope=activity:read_all";
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

  private async Task<ActionResult<T>> CallStravaApi<T>(string url)
  {
    var tokenJson = HttpContext.Session.GetString("StravaToken");
    if (string.IsNullOrEmpty(tokenJson)) return Unauthorized();

    var token = JsonSerializer.Deserialize<StravaAuthResponse>(tokenJson);
    if (token == null) return Unauthorized();

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Access_Token);

    var response = await _http.SendAsync(request);
    if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<T>(json);

    if (result == null) return NotFound();
    return Ok(result);
  }

  [HttpGet("latest-activity")]
  public async Task<ActionResult> LatestActivity()
  {
    var result = await CallStravaApi<List<StravaActivity>>(
        "https://www.strava.com/api/v3/athlete/activities?per_page=1&page=1");

    if (result.Result is not OkObjectResult ok) return result.Result!;

    var activities = ok.Value as List<StravaActivity>;
    if (activities == null || activities.Count == 0) return NotFound();

    return Ok(DtoMapper.ToDto(activities[0]));
  }
}
