export interface StravaAthlete {
  id: number;
  username: string;
  full_name: string;
  profile_image: string;
}

export interface StravaActivity {
  id: number;
  name: string;
  sport_type: string;
  distance: number;        // meters
  moving_time: number;     // seconds
  start_date_utc: string;  // ISO 8601 date/time
  encoded_route: string;   // activity route with Google Polyline Encoding
}
