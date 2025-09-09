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
}
