import React, { useState, useEffect } from "react";
import "./AdminDashboard.css";
import {
  User,
  Users,
  Building2,
  Calendar,
  LogOut,
  CreditCard,
  DollarSign,
  Wallet2,
} from "lucide-react";

import Employees from "./Employees";
import Departments from "./Departments";
import Leaves from "./Leaves";
import AddEmployee from "./AddEmployee";
import Payrolls from "./Payrolls";
import PayrollDetail from "./PayrollDetail";
import AddPayrollDetail from "./AddPayrollDetail";
import Compensation from "./Compensation";

const menu = [
  {
    section: "MAIN",
    items: [{ key: "dashboard", label: "Dashboard", icon: <Users size={20} /> }],
  },
  {
    section: "ACTIONS",
    items: [
      { key: "employees", label: "Employees", icon: <Users size={20} /> },
      { key: "departments", label: "Departments", icon: <Building2 size={20} /> },
      { key: "leaves", label: "Leaves", icon: <Calendar size={20} /> },
      { key: "payrolls", label: "Payrolls", icon: <CreditCard size={20} /> },
      { key: "payrolldetails", label: "PayrollDetails", icon: <DollarSign size={20} /> },
      { key: "compensations", label: "Compensations", icon: <Wallet2 size={20} /> }
    ],
  },
  {
    section: "OTHERS",
    items: [{ key: "logout", label: "Logout", icon: <LogOut size={20} /> }],
  },
];

export default function AdminDashboard() {
  const [active, setActive] = useState("dashboard");
  const [stats, setStats] = useState({
    totalEmployees: 0,
    activeEmployees: 0,
    totalDepartments: 0,
    pendingLeaves: 0,
  });
  const [showAddEmployee, setShowAddEmployee] = useState(false);
  const [showAddPayrollDetail, setShowAddPayrollDetail] = useState(false);

  useEffect(() => {
    if (active === "dashboard") {
      fetch("https://localhost:7058/api/Stats/counts")
        .then(res => {
          if (!res.ok) throw new Error("Network response was not ok");
          return res.json();
        })
        .then(data => {
          setStats({
            totalEmployees: data.totalEmployees ?? 0,
            activeEmployees: data.activeEmployees ?? 0,
            totalDepartments: data.totalDepartments ?? 0,
            pendingLeaves: data.pendingLeaves ?? 0,
          });
        })
        .catch(err => {
          console.error("Error fetching dashboard stats:", err);
        });
    }
  }, [active]);

  const handleCloseAddEmployee = () => setShowAddEmployee(false);
  const handleCloseAddPayrollDetail = () => setShowAddPayrollDetail(false);

  const handleLogout = () => {
    localStorage.clear();
    window.location.href = "/login";
  };

  const handleMenuClick = key => {
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
          <span className="brand">Admin Panel</span>
        </div>
        {menu.map(({ section, items }) => (
          <React.Fragment key={section}>
            <div className="sidebar-section-title">{section}</div>
            <ul>
              {items.map(item => (
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
                  {item.icon} <span>{item.label}</span>
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
              : active.charAt(0).toUpperCase() + active.slice(1)}
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
          {active === "dashboard" && (
            <DashboardCards setActive={setActive} stats={stats} />
          )}
          {active === "employees" && (
            <>
              <button
                className="add-employee-button"
                onClick={() => setShowAddEmployee(true)}
              >
                Add Employee
              </button>
              <Employees />
              {showAddEmployee && (
                <AddEmployee onCancel={handleCloseAddEmployee} />
              )}
            </>
          )}
          {active === "departments" && <Departments />}
          {active === "leaves" && <Leaves />}
          {active === "payrolls" && <Payrolls />}
          {active === "payrolldetails" && (
            <>
              <button
                className="add-payrolldetail-btn"
                onClick={() => setShowAddPayrollDetail(true)}
                style={{ marginBottom: "12px" }}
              >
                Add Payroll Detail
              </button>
              <PayrollDetail />
              {showAddPayrollDetail && (
                <AddPayrollDetail onClose={handleCloseAddPayrollDetail} />
              )}
            </>
          )}
          {active === "compensations" && (
            <Compensation />
          )}
        </div>
      </div>
    </div>
  );
}

function DashboardCards({ setActive, stats }) {
  const {
    totalEmployees,
    activeEmployees,
    totalDepartments,
    pendingLeaves
  } = stats;

  const statsData = [
    {
      label: "Total Employees",
      value: totalEmployees,
      icon: <Users size={32} color="#3b82f6" />,
      color: "#e0f2fe"
    },
    {
      label: "Active Employees",
      value: activeEmployees,
      icon: <User size={32} color="#10b981" />,
      color: "#dcfce7"
    },
    {
      label: "Departments",
      value: totalDepartments,
      icon: <Building2 size={32} color="#8b5cf6" />,
      color: "#ede9fe"
    },
    {
      label: "Pending Leaves",
      value: pendingLeaves,
      icon: <Calendar size={32} color="#f59e42" />,
      color: "#ffecd5"
    }
  ];

  return (
    <>
      <div className="dashboard-welcome">
        Welcome back! Here's what's happening in your organization.
      </div>
      <div className="dashboard-stats">
        {statsData.map(stat => (
          <div className="card stat-card" key={stat.label}>
            <div>
              <div className="stat-card-label">{stat.label}</div>
              <div className="stat-card-value">{stat.value}</div>
            </div>
            <div className="stat-card-icon" style={{ background: stat.color }}>
              {stat.icon}
            </div>
          </div>
        ))}
      </div>
      <div className="quick-actions">
        <div className="qa-title">Quick Actions</div>
        <div className="qa-list">
          <div
            className="qa-card"
            onClick={() => setActive("employees")}
            style={{ cursor: "pointer" }}
          >
            <Users size={32} color="#3b82f6" />
            <div className="qa-label">Add Employee</div>
            <div className="qa-desc">Add a new employee to the system</div>
          </div>
          <div
            className="qa-card"
            onClick={() => setActive("departments")}
            style={{ cursor: "pointer" }}
          >
            <Building2 size={32} color="#10b981" />
            <div className="qa-label">Add Department</div>
            <div className="qa-desc">Create a new department</div>
          </div>
          <div
            className="qa-card"
            onClick={() => setActive("leaves")}
            style={{ cursor: "pointer" }}
          >
            <Calendar size={32} color="#f59e42" />
            <div className="qa-label">Review Leaves</div>
            <div className="qa-desc">Review pending leave requests</div>
          </div>
        </div>
      </div>
    </>
  );
}
