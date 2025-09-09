namespace weather_server.Services;

using System.Collections.Concurrent;

using weather_server.Models;

using AthleteId = long;

public class StravaAthleteCache
{
  // key = athlete ID
  private readonly ConcurrentDictionary<AthleteId, StravaAuthResponse> _tokens = new();

  public void Save(StravaAuthResponse token)
  {
    _tokens[token.Athlete.Id] = token;
  }

  public StravaAuthResponse? Get(AthleteId athleteId)
  {
    return _tokens.TryGetValue(athleteId, out var token) ? token : null;
  }

  public void Delete(AthleteId athleteId)
  {
    _tokens.TryRemove(athleteId, out _);
  }
}
