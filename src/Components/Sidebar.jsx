import React from "react";
import { Link, useLocation } from "react-router-dom";
import { 
  User, 
  Users, 
  Building2, 
  Calendar, 
  BarChart3, 
  Menu,
  ChevronLeft,
  Settings,
  LogOut
} from "lucide-react";

const Sidebar = ({ collapsed, onToggle }) => {
  const location = useLocation();

  const menuItems = [
    {
      title: "Dashboard",
      icon: BarChart3,
      path: "/",
      section: "main"
    },
    {
      title: "Profile",
      icon: User,
      path: "/profile",
      section: "main"
    },
    {
      title: "Employees",
      icon: Users,
      path: "/employees",
      section: "actions"
    },
    {
      title: "Departments",
      icon: Building2,
      path: "/departments",
      section: "actions"
    },
    {
      title: "Leaves",
      icon: Calendar,
      path: "/leaves",
      section: "actions"
    }
  ];

  const otherItems = [
    {
      title: "Settings",
      icon: Settings,
      path: "/settings"
    },
    {
      title: "Logout",
      icon: LogOut,
      path: "/logout"
    }
  ];

  const isActive = (path) => {
    if (path === "/" && location.pathname === "/") return true;
    if (path !== "/" && location.pathname.startsWith(path)) return true;
    return false;
  };

  return (
    <div className={`fixed left-0 top-0 h-full bg-white border-r border-slate-200 sidebar-transition z-50 ${
      collapsed ? "w-16" : "w-64"
    }`}>
      {/* Header */}
      <div className="flex items-center justify-between p-4 border-b border-slate-200">
        {!collapsed && (
          <h2 className="text-xl font-bold text-slate-800">
            Admin Panel
          </h2>
        )}
        <button
          onClick={onToggle}
          className="p-2 rounded-lg hover:bg-slate-100 transition-colors"
          data-testid="sidebar-toggle-btn"
        >
          {collapsed ? <Menu size={20} /> : <ChevronLeft size={20} />}
        </button>
      </div>

      {/* Navigation */}
      <nav className="flex-1 px-4 py-6">
        {/* Main Section */}
        <div className="mb-8">
          {!collapsed && (
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">
              Main
            </p>
          )}
          <div className="space-y-2">
            {menuItems.filter(item => item.section === "main").map((item) => (
              <Link
                key={item.path}
                to={item.path}
                className={`flex items-center px-3 py-2.5 rounded-lg transition-all duration-200 group ${
                  isActive(item.path)
                    ? "bg-blue-50 text-blue-700 border-r-2 border-blue-700"
                    : "text-slate-600 hover:bg-slate-50 hover:text-slate-900"
                }`}
                data-testid={`nav-${item.title.toLowerCase()}`}
              >
                <item.icon size={20} className="flex-shrink-0" />
                {!collapsed && (
                  <span className="ml-3 font-medium">{item.title}</span>
                )}
              </Link>
            ))}
          </div>
        </div>

        {/* Actions Section */}
        <div className="mb-8">
          {!collapsed && (
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">
              Actions
            </p>
          )}
          <div className="space-y-2">
            {menuItems.filter(item => item.section === "actions").map((item) => (
              <Link
                key={item.path}
                to={item.path}
                className={`flex items-center px-3 py-2.5 rounded-lg transition-all duration-200 group ${
                  isActive(item.path)
                    ? "bg-blue-50 text-blue-700 border-r-2 border-blue-700"
                    : "text-slate-600 hover:bg-slate-50 hover:text-slate-900"
                }`}
                data-testid={`nav-${item.title.toLowerCase()}`}
              >
                <item.icon size={20} className="flex-shrink-0" />
                {!collapsed && (
                  <span className="ml-3 font-medium">{item.title}</span>
                )}
              </Link>
            ))}
          </div>
        </div>

        {/* Others Section */}
        <div>
          {!collapsed && (
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">
              Others
            </p>
          )}
          <div className="space-y-2">
            {otherItems.map((item) => (
              <Link
                key={item.path}
                to={item.path}
                className="flex items-center px-3 py-2.5 rounded-lg text-slate-600 hover:bg-slate-50 hover:text-slate-900 transition-all duration-200 group"
                data-testid={`nav-${item.title.toLowerCase()}`}
              >
                <item.icon size={20} className="flex-shrink-0" />
                {!collapsed && (
                  <span className="ml-3 font-medium">{item.title}</span>
                )}
              </Link>
            ))}
          </div>
        </div>
      </nav>
    </div>
  );
};

export default Sidebar;