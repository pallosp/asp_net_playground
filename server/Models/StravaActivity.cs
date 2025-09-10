using System.Text.Json.Serialization;

public class StravaActivity
{
  [JsonPropertyName("name")]
  public required string Name { get; set; } = "";
  [JsonPropertyName("type")]
  public required string SportType { get; set; } = "";
  [JsonPropertyName("distance")]
  public float Distance { get; set; } = 0; // meters
  [JsonPropertyName("moving_time")]
  public int MovingTime { get; set; } = 0; // seconds
  [JsonPropertyName("start_date")]
  public required DateTime StartDateUtc { get; set; }
}
