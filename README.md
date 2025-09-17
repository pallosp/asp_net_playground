# ASP.NET playground

Random weather forecast + Strava API + OSM demo

## Cheat sheet

### Publishing to gcloud

From the `server/` directory

```sh
dotnet publish -c Release
gcloud compute scp --recurse bin/Release/net9.0/publish/* asp-net-demo:~/app
```

### Running on Compute Engine

**While the SSH session lasts**

```sh
gcloud compute ssh asp-net-demo

# Copy from https://strava.com/settings/api
export Strava__ClientSecret="..."

cd ~/app
dotnet server.dll --urls "http://0.0.0.0:5252"
```

**Until VM reboot**

```sh
nohup dotnet server.dll --urls "http://0.0.0.0:5252" | tee -a server.log
```

### Website

- http://localhost:5173/ (served by `npm run dev`)
- http://localhost:5252/ (served by `dotnet run`)
- http://34.72.225.123:5252/ (compute engine IP printed by
  `gcloud compute instances list`)
- http://pallosp.mywire.org:5252/ (dynamic DNS registered at
  [dynu](https://www.dynu.com/))
