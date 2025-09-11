namespace server.Models;

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

  [JsonPropertyName("map")]
  public StravaMap? Map { get; set; }
}

public class StravaMap
{
  [JsonPropertyName("summary_polyline")]
  public string EncodedRoute { get; set; } = "";
}

public class StravaAthlete
{
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("username")]
  public required string Username { get; set; }

  [JsonPropertyName("firstname")]
  public required string FirstName { get; set; }

  [JsonPropertyName("lastname")]
  public string LastName { get; set; } = "";

  [JsonPropertyName("profile_medium")]
  public required string ProfileImage { get; set; }
}

public class StravaAuthResponse
{
  [JsonPropertyName("token_type")]
  public required string Token_Type { get; set; }

  [JsonPropertyName("expires_at")]
  public long Expires_At { get; set; }

  [JsonPropertyName("expires_in")]
  public int Expires_In { get; set; }

  [JsonPropertyName("refresh_token")]
  public required string Refresh_Token { get; set; }

  [JsonPropertyName("access_token")]
  public required string Access_Token { get; set; }

  [JsonPropertyName("athlete")]
  public required StravaAthlete Athlete { get; set; }
}
