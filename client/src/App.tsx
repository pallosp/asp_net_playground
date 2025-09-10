import { WeatherDemo } from "./WeatherDemo";
import { StravaApiDemo } from "./StravaApiDemo";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import LatestActivity from "./LatestActivity";

function MainPage() {
  return (
    <>
      <WeatherDemo />
      <StravaApiDemo />
    </>
  );
}

function App() {
  return (
    <div style={{ padding: 20 }}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/latest" element={<LatestActivity />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
