using System.Text.Json.Serialization;

public class StravaActivityDto
{
  [JsonPropertyName("name")]
  public string Name { get; set; } = "";

  [JsonPropertyName("sport_type")]
  public string SportType { get; set; } = "";

  [JsonPropertyName("distance")]
  public float Distance { get; set; } // meters

  [JsonPropertyName("moving_time")]
  public int MovingTime { get; set; } // seconds

  [JsonPropertyName("start_date_utc")]
  public DateTime StartDateUtc { get; set; }
}
