import { useEffect, useState } from "react";
import type { Weather } from "./model/weather";

export function WeatherDemo() {
  const [weather, setWeather] = useState<Weather[]>([]);

  useEffect(() => {
    fetch("/api/v1/weather")
      .then((res) => res.json())
      .then(setWeather)
      .catch(console.error);
  }, []);

  return (
    <>
      <h1>Weather Forecast</h1>
      <ul>
        {weather.map((w, i) => (
          <li key={i}>
            {w.date} — {w.temperatureC} °C — {w.summary}
          </li>
        ))}
      </ul>
    </>
  );
}
