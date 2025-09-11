namespace server.Controllers;

using Microsoft.AspNetCore.Mvc;
using server.Models;

[ApiController]
[Route("api/v1/[controller]")]
public class WeatherController : ControllerBase
{
  private static readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  [HttpGet]
  public ActionResult<IEnumerable<WeatherForecastDto>> Get([FromQuery] int days = 5)
  {
    if (days < 1 || days > 14)
      return BadRequest("Invalid number of days");

    var forecasts = Enumerable.Range(1, days).Select(index =>
        new WeatherForecastDto(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        ))
        .ToArray();
    return Ok(forecasts);
  }
}
