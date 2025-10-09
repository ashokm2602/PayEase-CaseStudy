import React, { useState } from 'react';
import Profile from './Profile';
import Employees from './Employees';
import Departments from './Departments';
import Leaves from './Leaves';
import './adminHome.css';

const AdminHome = () => {
  const [activeTab, setActiveTab] = useState('profile');
  const [actionsOpen, setActionsOpen] = useState(true);

  const renderContent = () => {
    switch (activeTab) {
      case 'profile':
        return <Profile />;
      case 'employees':
        return <Employees />;
      case 'departments':
        return <Departments />;
      case 'leaves':
        return <Leaves />;
      case 'others':
        return <div><h2>Other Settings</h2><p>Coming soon...</p></div>;
      default:
        return null;
    }
  };

  return (
    <div className="admin-home">
      <aside className="sidebar">
        <h2>Admin Panel</h2>
        <ul className="sidebar-menu">
          <li
            className={activeTab === 'profile' ? 'active' : ''}
            onClick={() => setActiveTab('profile')}
          >
            Profile
          </li>
          <li
            className="actions-header"
            onClick={() => setActionsOpen(!actionsOpen)}
            style={{cursor: "pointer"}}
          >
            Actions {actionsOpen ? '▲' : '▼'}
          </li>
          {actionsOpen && (
            <ul className="actions-submenu">
              <li
                className={activeTab === 'employees' ? 'active' : ''}
                onClick={() => setActiveTab('employees')}
              >
                Employees
              </li>
              <li
                className={activeTab === 'departments' ? 'active' : ''}
                onClick={() => setActiveTab('departments')}
              >
                Departments
              </li>
              <li
                className={activeTab === 'leaves' ? 'active' : ''}
                onClick={() => setActiveTab('leaves')}
              >
                Leaves
              </li>
            </ul>
          )}
          <li
            className={activeTab === 'others' ? 'active' : ''}
            onClick={() => setActiveTab('others')}
          >
            Others
          </li>
        </ul>
      </aside>

      <main className="content-area">{renderContent()}</main>
    </div>
  );
};

export default AdminHome;
