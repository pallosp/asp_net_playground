namespace server.Services;

using server.Models;

public static class DtoMapper
{
  public static StravaAthleteDto ToDto(StravaAthlete athlete)
  {
    return new StravaAthleteDto
    {
      Id = athlete.Id.ToString(),
      Username = athlete.Username,
      FullName = $"{athlete.FirstName} {athlete.LastName}".Trim(),
      ProfileImage = athlete.ProfileImage
    };
  }

  public static StravaActivityDto ToDto(StravaActivity activity)
  {
    return new StravaActivityDto
    {
      Name = activity.Name,
      SportType = activity.SportType,
      Distance = activity.Distance,
      MovingTime = activity.MovingTime,
      StartDateUtc = activity.StartDateUtc
    };
  }
}
