import React, { useEffect, useState } from "react";
import * as jwt_decode from "jwt-decode";
import "./profile.css";

function getCurrentUserId() {
  const token = localStorage.getItem("token");
  if (!token) return null;
  try {
    const decoded = jwt_decode(token);
    return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  } catch {
    return null;
  }
}

// rest of Profile.jsx as before


function Profile() {
  const [profile, setProfile] = useState({
    userId: "",
    username: "",
    role: ""
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const token = localStorage.getItem("token");
  const userId = getCurrentUserId();

  useEffect(() => {
    async function fetchUser() {
      setLoading(true);
      setError("");
      try {
        const res = await fetch(`https://localhost:7058/api/Users/getuser/${userId}`, {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
          }
        });
        if (!res.ok) throw new Error("Failed to fetch user data");
        const data = await res.json();
        setProfile({
          userId: data.userId || "",
          username: data.username || data.userName || "",
          role: data.role || ""
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    if (userId) {
      fetchUser();
    } else {
      setError("User ID is not available");
      setLoading(false);
    }
  }, [userId, token]);

  if (loading) return <div>Loading profile...</div>;
  if (error) return <div className="error-message">{error}</div>;

  return (
    <div className="profile-container">
      <div className="profile-details">
        <div className="profile-avatar">
          <span className="avatar-letter">
            {(profile.username && profile.username.charAt(0).toUpperCase()) || "U"}
          </span>
        </div>
        <div className="profile-info">
          <h2>{profile.username}</h2>
          <p className="profile-role">{profile.role}</p>
          <p><strong>User ID:</strong> {profile.userId}</p>
        </div>
      </div>
    </div>
  );
}

export default Profile;
