import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./login.css";

export default function Login({ onLogin }) {
  const [credentials, setCredentials] = useState({
    username: "",
    password: "",
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      // 1) Login
      const res = await axios.post(
        "https://localhost:7058/api/Authentication/login",
        credentials,
        { validateStatus: (status) => status < 500 } // let us inspect 4xx responses
      );

      console.log("Login response status:", res.status);
      console.log("Login response data:", res.data);

      if (res.status !== 200) {
        // Provide user-friendly error and also log details
        alert("Login failed. Check username/password.");
        return;
      }

      const { token, refreshToken, username, roles, expiration, userId } = res.data;

      // Save token only after successful login response
      const prevToken = localStorage.getItem("token");

      localStorage.setItem("token", token);
      localStorage.setItem("refreshToken", refreshToken);
      localStorage.setItem("username", username);
      localStorage.setItem("roles", JSON.stringify(roles));
      localStorage.setItem("expiration", expiration);
      localStorage.setItem("userId", userId);

      // 2) Fetch employee by userId (pass token in header).
      // Allow 404 so we can handle it gracefully and inspect result
      let empRes;
      try {
        empRes = await axios.get("https://localhost:7058/api/Employees/GetEmployeeByUserId", {
          params: { userId },
          headers: { Authorization: `Bearer ${token}` },
          validateStatus: (status) => status < 500,
        });

        console.log("GetEmployeeByUserId status:", empRes.status);
        console.log("GetEmployeeByUserId data:", empRes.data);

        if (empRes.status === 200 && empRes.data) {
          // The backend Employee model uses EmpId (C#). Try multiple property names just in case.
          const data = empRes.data;
          const employeeId =
            data.empId ??
            data.EmpId ??
            data.id ??
            data.employeeId ??
            data.EmployeeId ??
            null;

          if (employeeId) {
            localStorage.setItem("employeeId", employeeId);
            console.log("Saved employeeId:", employeeId);
          } else {
            console.warn("Employee object returned but no recognizable ID field found. Inspect empRes.data above.");
          }
        } else if (empRes.status === 404) {
          console.warn("No employee found for this user (404).");
        } else {
          console.warn("Unexpected response fetching employee:", empRes.status, empRes.data);
        }
      } catch (empErr) {
        // network errors etc
        console.error("Network/error while fetching employee:", empErr);
      }

      // 3) Call parent onLogin only if token changed — helps prevent repeated identical updates
      try {
        if (typeof onLogin === "function") {
          if (!prevToken || prevToken !== token) {
            onLogin(token);
          } else {
            // token unchanged — avoid calling onLogin to prevent triggering parent update loops
            console.log("Token unchanged; skipping onLogin to avoid potential rerender loop.");
          }
        }
      } catch (cbErr) {
        console.error("Error calling onLogin callback:", cbErr);
      }

      // navigate once after login attempt
      navigate("/");
    } catch (err) {
      console.error("Login error:", err);
      alert("Login failed! See console for details.");
    }
  };

  return (
    <div className="login-page">
      <form className="login-card" onSubmit={handleSubmit}>
        <h2>Login to PayrollPro</h2>

        <label htmlFor="username">Username</label>
        <input
          id="username"
          type="text"
          name="username"
          value={credentials.username}
          onChange={handleChange}
          placeholder="Enter your username"
          required
          autoComplete="username"
        />

        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          name="password"
          value={credentials.password}
          onChange={handleChange}
          placeholder="Enter your password"
          required
          autoComplete="current-password"
        />

        <button type="submit">Login</button>
      </form>
    </div>
  );
}
