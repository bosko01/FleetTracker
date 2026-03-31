import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { authStorage } from '../../services/authStorage';

const menu = [
  ['Dashboard', '/dashboard'],
  ['Vehicles', '/vehicles'],
  ['Drivers', '/drivers'],
  ['Tours', '/tours'],
  ['Statistics', '/statistics'],
] as const;

export function AdminLayout() {
  const nav = useNavigate();
  return <div className="layout">
    <aside className="sidebar">
      <h3>FleetTracker</h3>
      {menu.map(([label, href]) => <NavLink key={href} to={href} className={({ isActive }) => isActive ? 'active' : ''}><span className="label">{label}</span></NavLink>)}
    </aside>
    <main className="main">
      <div className="topbar">
        <h2>Admin Portal</h2>
        <button className="button secondary" onClick={() => { authStorage.clear(); nav('/login'); }}>Logout</button>
      </div>
      <Outlet />
    </main>
  </div>;
}
