import { useEffect, useState } from "react";
import type { User } from "./model/auth";

type LoginFormProps = {
  onLogin: (username: string, password: string) => void;
};

function LoginForm({ onLogin }: LoginFormProps) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault(); // prevent full page reload
    onLogin(username, password);
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      &ensp;
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      &ensp;
      <button type="submit">Login</button>
    </form>
  );
}

type LoggedInProps = {
  user: User;
  onLogout: () => void;
};

function LoggedIn({ user, onLogout }: LoggedInProps) {
  return (
    <div>
      <p>Welcome, {user.username}!</p>
      <button onClick={onLogout}>Logout</button>
    </div>
  );
}

export function AuthDemo() {
  const [user, setUser] = useState<User | null>(null);

  // Check login status on mount
  useEffect(() => {
    fetch("/auth/me", { credentials: "include" })
      .then((res) => (res.ok ? res.json() : null))
      .then((data: User | null) => {
        if (data) setUser(data);
      });
  }, []);

  const login = async (username: string, password: string) => {
    const res = await fetch("/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({ username, password }),
    });

    if (res.ok) {
      const data: User = await res.json();
      setUser(data);
    } else {
      alert("Invalid credentials (try demo/demo)");
    }
  };

  const logout = async () => {
    await fetch("/auth/logout", {
      method: "POST",
      credentials: "include",
    });
    setUser(null);
  };

  return (
    <>
      <h1>Authentication demo</h1>
      {user ? (
        <LoggedIn user={user} onLogout={logout} />
      ) : (
        <LoginForm onLogin={login} />
      )}
    </>
  );
}
