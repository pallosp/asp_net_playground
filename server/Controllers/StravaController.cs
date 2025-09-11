namespace server.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

using server.Models;
using server.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class StravaController : ControllerBase
{
  private readonly HttpClient _http;
  private readonly IConfiguration _config;
  private readonly ILogger<StravaController> _logger;
  private readonly IMemoryCache _cache;

  public StravaController(
    HttpClient http,
    IConfiguration config,
    ILogger<StravaController> logger,
    IMemoryCache cache)
  {
    _http = http;
    _config = config;
    _logger = logger;
    _cache = cache;
  }

  [HttpGet("connect")]
  public IActionResult Connect()
  {
    var clientId = _config["Strava:ClientId"];
    var redirectUrl = Url.Action(
        "ConnectCallback",
        ControllerContext.ActionDescriptor.ControllerName,
        null,
        Request.Scheme
    );
    var url = $"https://www.strava.com/oauth/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUrl}&scope=activity:read_all";
    return Redirect(url);
  }

  [HttpGet("connect-callback")]
  public async Task<ActionResult> ConnectCallback([FromQuery] string code)
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
  public ActionResult<StravaAthleteDto> Me()
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
  public async Task<ActionResult<StravaActivityDto>> LatestActivity()
  {
    var cacheKey = "LatestActivity";
    if (_cache.TryGetValue(cacheKey, out var cachedResponse))
    {
      _logger.LogDebug("Cache hit for latest activity");
      return Ok(cachedResponse);
    }

    _logger.LogDebug("Fetching latest activity from Strava");
    var result = await CallStravaApi<List<StravaActivity>>(
        "https://www.strava.com/api/v3/athlete/activities?per_page=1&page=1");

    if (result.Result is not OkObjectResult ok) return result.Result!;

    var activities = ok.Value as List<StravaActivity>;
    if (activities == null || activities.Count == 0) return NotFound();

    var dto = DtoMapper.ToDto(activities[0]);
    _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(5));
    return Ok(dto);
  }
}
