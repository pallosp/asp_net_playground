import { useEffect, useState } from "react";

type Weather = {
  date: string;
  temperatureC: number;
  summary: string;
};

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
    </div>
  );
}

export default App;
