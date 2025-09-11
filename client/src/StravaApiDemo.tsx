import { useEffect, useState } from "react";
import type { StravaAthlete } from "./model/strava";
import { Link } from "react-router-dom";

export function StravaApiDemo() {
  const [athlete, setAthlete] = useState<StravaAthlete | null>(null);

  useEffect(() => {
    fetch("/api/v1/strava/me")
      .then((res) => {
        if (!res.ok) return null; // unauthorized or not connected
        return res.json();
      })
      .then((data: StravaAthlete | null) => setAthlete(data))
      .catch((err) => console.error(err));
  }, []);

  const handleConnect = () => {
    window.location.href = "/api/v1/strava/connect?returnUrl=/";
  };

  const handleDisconnect = () => {
    fetch("/api/v1/strava/disconnect", { method: "POST" })
      .then(() => {
        setAthlete(null);
        window.history.replaceState({}, document.title, "/");
      })
      .catch((err) => console.error(err));
  };

  if (!athlete) {
    return (
      <div style={{ marginTop: "3rem" }}>
        <h1>Strava API demo</h1>
        <button onClick={handleConnect}>Connect with Strava</button>
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
      <Link to="/latest">
        <button>See latest activity</button>
      </Link>
      <button onClick={handleDisconnect}>Disconnect</button>
    </div>
  );
}
