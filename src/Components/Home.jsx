import React from "react";
import "./home.css";


const Home = () => {
  return (
    <div className="home-page">
      <header className="navbar">
        <div className="brand">
          {/*          <img src="" alt="PayrollPro Logo" className="brand-logo" />*/}
          <span className="brand-name">PayrollPro</span>
        </div>
        <nav className="nav-actions">
          <a className="nav-link" href="#features">Features</a>
          <a className="nav-link" href="#pricing">Pricing</a>
          <a className="btn-login" href="/login">Login</a>
        </nav>
      </header>

      <main className="hero">
        <section className="hero-content">
          <h1>Run Payroll in Minutes</h1>
          <p className="subtitle">
            Accurate, compliant, and automated payroll for growing teams—taxes, payslips,
            and filings handled seamlessly.
          </p>
          <div className="hero-ctas">
            <a className="btn-primary" href="/signup">Get Started</a>
            <a className="btn-secondary" href="#features">See Features</a>
          </div>
        </section>

        <section id="features" className="features-grid">
          <article className="card">
            <h3>Automated Payroll</h3>
            <p>Schedule runs, prorate salaries, and auto-calc overtime and bonuses with one click.</p>
          </article>
          <article className="card">
            <h3>Tax & Compliance</h3>
            <p>Built-in tax tables, statutory deductions, and automated filings to stay compliant.</p>
          </article>
          <article className="card">
            <h3>Payslips & Reports</h3>
            <p>Generate digital payslips and export payroll summaries, GL, and audit trails.</p>
          </article>
          <article className="card">
            <h3>Employee Self-Service</h3>
            <p>Let employees view payslips, leave balances, and update bank details securely.</p>
          </article>
          <article className="card">
            <h3>Time & Attendance</h3>
            <p>Import timesheets and sync attendance to calculate hours and on-costs.</p>
          </article>
          <article className="card">
            <h3>Integrations</h3>
            <p>Connect with accounting systems and HRIS for a unified, real-time workflow.</p>
          </article>
        </section>

        <section id="pricing" className="info-strip">
          <div className="info-item">
            <span className="info-kpi">99.95%</span>
            <span className="info-label">Uptime</span>
          </div>
          <div className="info-item">
            <span className="info-kpi">3 min</span>
            <span className="info-label">Avg. run</span>
          </div>
          <div className="info-item">
            <span className="info-kpi">AES-256</span>
            <span className="info-label">Encryption</span>
          </div>
        </section>
      </main>

      <footer className="footer">
        <p>© {new Date().getFullYear()} PayrollPro. All rights reserved.</p>
      </footer>
    </div>
  );
};

export default Home;
