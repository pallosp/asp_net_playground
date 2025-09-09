using System.Text.Json.Serialization;

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

public class StravaAthlete
{
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("username")]
  public required string UserName { get; set; }

  [JsonPropertyName("firstname")]
  public required string FirstName { get; set; }

  [JsonPropertyName("profile_medium")]
  public required string ProfilePicture { get; set; }
}
