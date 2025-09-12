# ASP.NET playground

Random weather forecast + Strava API + OSM demo

## Cheat sheet

### Publishing to gcloud

From the `server/` directory

```sh
dotnet publish -c Release
gcloud compute scp --recurse bin/Release/net9.0/publish/* asp-net-demo:~/app
```
