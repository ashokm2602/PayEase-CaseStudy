import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";

export default function RequireAuth({ children, allowedRoles = [] }) {
  const [isReady, setIsReady] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [hasRole, setHasRole] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const roles = JSON.parse(localStorage.getItem("roles") || "[]");

    setIsAuthenticated(!!token);

    // Check if user has at least one allowed role
    if (allowedRoles.length === 0) {
      setHasRole(true); // no role restriction
    } else {
      const match = roles.some((role) => allowedRoles.includes(role));
      setHasRole(match);
    }

    setIsReady(true);
  }, [allowedRoles]);

  if (!isReady) return null; // or show a loader

  if (!isAuthenticated) {
    // Not logged in
    return <Navigate to="/login" replace />;
  }

  if (!hasRole) {
    // Logged in but does not have required role
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
}
