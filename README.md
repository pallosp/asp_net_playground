# ASP.NET playground

Random weather forecast + Strava API + OSM demo

## Cheat sheet

### Publishing to gcloud

From the `server/` directory

```sh
dotnet publish -c Release
gcloud compute scp --recurse bin/Release/net9.0/publish/* asp-net-demo:~/app
```

### Running the server

```
gcloud compute ssh asp-net-demo
cd ~/app
dotnet server.dll --urls "http://0.0.0.0:5252"
```

### Website

- http://localhost:5173/ (served by `npm run dev`)
- http://localhost:5252/ (served by `dotnet run`)
- http://34.72.225.123:5252/ (compute engine IP printed by
  `gcloud compute instances list`)
- http://pallosp.mywire.org:5252/ (dynamic DNS registered at
  [dynu](https://www.dynu.com/))
