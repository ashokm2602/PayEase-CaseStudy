import { useState, useEffect } from "react";
import { BrowserRouter, Routes, Route, Navigate, useNavigate } from "react-router-dom";
import Login from "./Components/login";
import AdminDashboard from "./Components/AdminDashboard";
import RequireAuth from "./Components/RequireAuth";
import EmployeeDashboard from "./Components/Employees/EmployeeDashboard";
import { Provider } from "react-redux";
import store from "./store";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function RoleBasedRedirect({ token, roles }) {
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }
    if (roles.includes("Admin") || roles.includes("Manager")) {
      navigate("/admin");
    } else if (roles.includes("Employee")) {
      navigate("/employee");
    } else {
      navigate("/login");
    }
  }, [token, roles, navigate]);

  return null;
}

function App() {
  const [token, setToken] = useState(() => {
    try {
      return localStorage.getItem("token") || null;
    } catch {
      return null;
    }
  });

  const [roles, setRoles] = useState(() => {
    try {
      const storedRoles = localStorage.getItem("roles");
      if (!storedRoles || storedRoles === "undefined") return [];
      return JSON.parse(storedRoles);
    } catch {
      return [];
    }
  });

  useEffect(() => {
    try {
      const storedRoles = localStorage.getItem("roles");
      if (!storedRoles || storedRoles === "undefined") {
        setRoles([]);
      } else {
        setRoles(JSON.parse(storedRoles));
      }
    } catch {
      setRoles([]);
    }
  }, [token]);

  const handleLogout = () => {
    localStorage.clear();
    setToken(null);
    setRoles([]);
    window.location.href = "/login";
  };

  return (
    <Provider store={store}>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login onLogin={setToken} />} />
          <Route
            path="/admin/*"
            element={
              <RequireAuth allowedRoles={["Admin", "Manager"]}>
                <AdminDashboard onLogout={handleLogout} />
              </RequireAuth>
            }
          />
          <Route
            path="/employee"
            element={
              <RequireAuth allowedRoles={["Employee"]}>
                <EmployeeDashboard />
              </RequireAuth>
            }
          />
          <Route path="/" element={<RoleBasedRedirect token={token} roles={roles} />} />
        </Routes>
      </BrowserRouter>
      <ToastContainer position="top-right" autoClose={3000} />
    </Provider>
  );
}

export default App;
