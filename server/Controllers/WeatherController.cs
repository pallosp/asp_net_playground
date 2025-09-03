using Microsoft.AspNetCore.Mvc;
using weather_server.Models;

namespace weather_server.Controllers;

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
  public IEnumerable<WeatherForecast> Get([FromQuery] int days = 5)
  {
    return Enumerable.Range(1, days).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        ))
        .ToArray();
  }
}

