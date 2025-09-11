namespace server.Models;

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

  [JsonPropertyName("encoded_route")]
  public string EncodedRoute { get; set; } = "";
}

public class StravaAthleteDto
{
  [JsonPropertyName("id")]
  public required string Id { get; set; } = string.Empty;

  [JsonPropertyName("username")]
  public required string Username { get; set; } = string.Empty;

  [JsonPropertyName("full_name")]
  public required string FullName { get; set; } = string.Empty;

  [JsonPropertyName("profile_image")]
  public required string ProfileImage { get; set; } = string.Empty;
}
