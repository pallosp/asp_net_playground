import { useEffect, useState } from "react";
import type { StravaActivity } from "./model/strava";
import polyline from "@mapbox/polyline";
import leaflet from "leaflet";
import { MapContainer, Marker, Polyline, TileLayer } from "react-leaflet";
import "leaflet/dist/leaflet.css";

export default function LatestActivity() {
  return (
    <div style={{ marginTop: "2rem" }}>
      <h2>Latest Activity</h2>
      <LatestActivityContent />
    </div>
  );
}

function LatestActivityContent() {
  const [activity, setActivity] = useState<StravaActivity | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<boolean>(false);

  useEffect(() => {
    fetch("/api/v1/strava/latest-activity")
      .then((res) => {
        if (!res.ok)
          throw new Error("Not connected or failed to load activity");
        return res.json();
      })
      .then((data) => {
        setActivity(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
        setError(true);
      });
  }, []);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error || !activity) {
    return <p>No activity found (or not connected to Strava)</p>;
  }

  const route = polyline.decode(activity.encoded_route);

  return (
    <>
      <p>
        <strong>{activity.name}</strong> ({activity.sport_type})
      </p>
      Distance: {(activity.distance / 1000).toFixed(1)} km
      <br />
      Moving time: {(activity.moving_time / 60).toFixed(0)} min
      <br />
      When: {new Date(activity.start_date_utc).toLocaleString()}
      <p />
      {route.length == 0 && "No route available"}
      {route.length > 0 && <ActivityMap route={route} />}
    </>
  );
}

function ActivityMap({ route }: { route: Array<[number, number]> }) {
  if (route.length === 0) return null;

  const start = route[0];
  const end = route[route.length - 1];
  const bounds = leaflet.latLngBounds(route);

  return (
    <MapContainer
      bounds={bounds}
      scrollWheelZoom={true}
      style={{ height: "400px", width: "600px" }}
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution="&copy; OpenStreetMap contributors"
      />
      <Polyline positions={route} color="red" />
      <Marker position={start} />
      <Marker position={end} />
    </MapContainer>
  );
}
