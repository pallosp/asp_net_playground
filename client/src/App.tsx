import { useEffect, useState } from "react";

type Weather = {
  date: string;
  temperatureC: number;
  summary: string;
};

function StravaLoginButton() {
  const handleLogin = () => {
    window.location.href = "/api/v1/stravaauth/login";
  };

  return (
    <div style={{ marginTop: "3rem" }}>
      <h1>Strava OAuth Demo</h1>
      <button onClick={handleLogin}>Connect with Strava</button>
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
      <StravaLoginButton />
    </div>
  );
}

export default App;
