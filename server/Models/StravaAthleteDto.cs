namespace weather_server.Models;

using System.Text.Json.Serialization;

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
