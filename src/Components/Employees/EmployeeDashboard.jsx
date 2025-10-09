import React, { useState, useEffect } from "react";
import "./EmployeeDashboard.css";
import { User, Users, Calendar, LogOut, DollarSign } from "lucide-react";
import EmployeeLeave from "./EmployeeLeaves";
import EmployeeProfile from "./EmployeeProfile";
import RequestLeave from "./RequestLeave";
import EmployeePayroll from "./EmployeePayroll";

const menu = [
  {
    section: "MAIN",
    items: [
      { key: "dashboard", label: "Dashboard", icon: <Users size={20} /> },
      { key: "profile", label: "Profile", icon: <User size={20} /> },
    ],
  },
  {
    section: "ACTIONS",
    items: [
      { key: "leaves", label: "My Leaves", icon: <Calendar size={20} /> },
      { key: "applyleave", label: "Apply Leave", icon: <Calendar size={20} /> },
      { key: "viewpayroll", label: "View Payroll", icon: <DollarSign size={20} /> },
    ],
  },
  {
    section: "OTHERS",
    items: [
      { key: "logout", label: "Logout", icon: <LogOut size={20} /> },
    ],
  },
];

export default function EmployeeDashboard() {
  const [active, setActive] = useState("dashboard");

  const handleLogout = () => {
    localStorage.clear();
    window.location.href = "/login";
  };

  const handleMenuClick = (key) => {
    if (key === "logout") {
      handleLogout();
    } else {
      setActive(key);
    }
  };

  return (
    <div className="admin-layout">
      <aside className="sidebar">
        <div className="sidebar-header">
          <span className="brand">Employee Panel</span>
        </div>
        {menu.map(({ section, items }) => (
          <React.Fragment key={section}>
            <div className="sidebar-section-title">{section}</div>
            <ul>
              {items.map((item) => (
                <li
                  key={item.key}
                  className={active === item.key ? "active" : ""}
                  onClick={() => handleMenuClick(item.key)}
                  style={
                    item.key === "logout"
                      ? { cursor: "pointer", color: "red" }
                      : { cursor: "pointer" }
                  }
                >
                  {item.icon}
                  <span>{item.label}</span>
                </li>
              ))}
            </ul>
          </React.Fragment>
        ))}
      </aside>
      <div className="main">
        <div className="main-header">
          <h2>
            {active === "dashboard"
              ? "Dashboard Overview"
              : active.charAt(0).toUpperCase() + active.slice(1).replace(/\B([A-Z])/g, " $1")}
          </h2>
          <div className="main-header-date">
            {new Date().toLocaleDateString("en-US", {
              weekday: "long",
              year: "numeric",
              month: "long",
              day: "numeric",
            })}
          </div>
        </div>
        <div className="main-content">
          {active === "dashboard" && <DashboardCards setActive={setActive} />}
          {active === "profile" && <EmployeeProfile />}
          {active === "leaves" && <EmployeeLeave />}
          {active === "applyleave" && <RequestLeave />}
          {active === "viewpayroll" && <EmployeePayroll />}
        </div>
      </div>
    </div>
  );
}

function DashboardCards({ setActive }) {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    pendingLeaves: 0,
    totalLeaves: 0,
    payrollAmount: 0,
  });
  const [error, setError] = useState("");

  const employeeId = localStorage.getItem("employeeId") || "4";
  const BASE_API_URL = "https://localhost:7058/api/EmployeeDashboard";

  useEffect(() => {
    async function fetchStats() {
      setLoading(true);
      setError("");
      try {
        if (!employeeId) {
          setError("No employeeId found in localStorage.");
          setLoading(false);
          return;
        }
        const response = await fetch(`${BASE_API_URL}/stats/${employeeId}`);
        const contentType = response.headers.get("content-type");
        if (!response.ok) {
          setError(`Network/API error: ${response.status}`);
        } else if (contentType && contentType.indexOf("application/json") !== -1) {
          const data = await response.json();
          setStats({
            pendingLeaves: data.pendingLeaves ?? 0,
            totalLeaves: data.totalLeaves ?? 0,
            payrollAmount: data.payrollAmount ?? 0,
          });
        } else {
          setError("API did not return JSON. Check endpoint and server.");
        }
      } catch (err) {
        setError("Network error: " + err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchStats();
  }, [employeeId]);

  const statsData = [
    {
      label: "Pending Leaves",
      value: stats.pendingLeaves,
      icon: <Calendar size={32} color="#f59e42" />,
      color: "#fff7e7",
      onClickKey: "applyleave",
    },
    {
      label: "Total Leaves",
      value: stats.totalLeaves,
      icon: <Calendar size={32} color="#3b82f6" />,
      color: "#e0f2fe",
      onClickKey: "leaves",
    },
    {
      label: "Payroll",
      value: `₹${parseFloat(stats.payrollAmount).toFixed(2)}`,
      icon: <DollarSign size={32} color="#10b981" />,
      color: "#dcfce7",
      onClickKey: "viewpayroll",
    },
  ];

  return (
    <>
      <div className="dashboard-welcome">Welcome! Here are your stats.</div>
      <div className="dashboard-stats">
        {loading ? (
          <div>Loading stats...</div>
        ) : error ? (
          <div style={{ color: "red" }}>{error}</div>
        ) : (
          statsData.map((stat) => (
            <div
              className="card stat-card"
              key={stat.label}
              onClick={() => setActive(stat.onClickKey)}
              style={{ cursor: "pointer" }}
            >
              <div>
                <div className="stat-card-label">{stat.label}</div>
                <div className="stat-card-value">{stat.value}</div>
              </div>
              <div className="stat-card-icon" style={{ background: stat.color }}>
                {stat.icon}
              </div>
            </div>
          ))
        )}
      </div>
      <div className="quick-actions">
        <div className="qa-title">Quick Actions</div>
        <div className="qa-list">
          <div className="qa-card" onClick={() => setActive("applyleave")} style={{ cursor: "pointer" }}>
            <Calendar size={32} color="#f59e42" />
            <div className="qa-label">Apply Leave</div>
            <div className="qa-desc">Submit a leave request</div>
          </div>
          <div className="qa-card" onClick={() => setActive("viewpayroll")} style={{ cursor: "pointer" }}>
            <DollarSign size={32} color="#10b981" />
            <div className="qa-label">View Payroll</div>
            <div className="qa-desc">See your salary info</div>
          </div>
          <div className="qa-card" onClick={() => setActive("profile")} style={{ cursor: "pointer" }}>
            <User size={32} color="#3b82f6" />
            <div className="qa-label">Profile</div>
            <div className="qa-desc">View or edit your info</div>
          </div>
        </div>
      </div>
    </>
  );
}
