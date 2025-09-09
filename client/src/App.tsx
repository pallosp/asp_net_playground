import { useEffect, useState } from "react";
import type { StravaAthlete } from "./model/strava";

type Weather = {
  date: string;
  temperatureC: number;
  summary: string;
};

function StravaApiDemo() {
  const [athlete, setAthlete] = useState<StravaAthlete | null>(null);
  const [athleteId, setAthleteId] = useState<string | null>(null);

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const id = params.get("athleteId");
    if (id) {
      setAthleteId(id);
      fetch(`/api/v1/stravaauth/me/${id}`)
        .then((res) => {
          if (!res.ok) throw new Error("Failed to load athlete");
          return res.json();
        })
        .then((data: StravaAthlete) => setAthlete(data))
        .catch((err) => console.error(err));
    }
  }, []);

  const handleLogin = () => {
    window.location.href = "/api/v1/stravaauth/login";
  };

  const handleDisconnect = () => {
    if (!athleteId) return;
    fetch(`/api/v1/stravaauth/disconnect/${athleteId}`, { method: "POST" })
      .then(() => {
        setAthlete(null);
        setAthleteId(null);
        window.history.replaceState({}, document.title, "/");
      })
      .catch((err) => console.error(err));
  };

  if (!athlete) {
    return (
      <div style={{ marginTop: "3rem" }}>
        <h1>Strava API demo</h1>
        <button onClick={handleLogin}>Connect with Strava</button>
      </div>
    );
  }

  return (
    <div style={{ marginTop: "3rem" }}>
      <h1>Welcome, {athlete.full_name}</h1>
      <img
        src={athlete.profile_image}
        alt="Profile picture"
        style={{ borderRadius: "50%", width: 120, height: 120 }}
      />
      <p>@{athlete.username}</p>
      <button onClick={handleDisconnect}>Disconnect</button>
    </div>
  );
}

function App() {
  const [weather, setWeather] = useState<Weather[]>([]);

  useEffect(() => {
    fetch("/api/v1/weather")
      .then((res) => res.json())
      .then(setWeather)
      .catch(console.error);
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h1>Weather Forecast</h1>
      <ul>
        {weather.map((w, i) => (
          <li key={i}>
            {w.date} — {w.temperatureC} °C — {w.summary}
          </li>
        ))}
      </ul>
      <StravaApiDemo />
    </div>
  );
}

export default App;
