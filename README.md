# ASP.NET playground

Random weather forecast + Strava API + OSM demo

## Cheat sheet

### Publishing to gcloud

From the `server/` directory

```sh
dotnet publish -c Release -o ../dist
gcloud compute scp --recurse ../dist/* asp-net-demo:~/app
```
