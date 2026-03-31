import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authApi } from '../services/api';
import { authStorage } from '../services/authStorage';

export function LoginPage() {
  const nav = useNavigate();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault(); setLoading(true); setError('');
    try { const response = await authApi.login(username, password); authStorage.set(response.accessToken); nav('/'); }
    catch { setError('Invalid credentials'); }
    finally { setLoading(false); }
  };

  return <div style={{ minHeight:'100vh', display:'grid', placeItems:'center' }}>
    <form className="card" style={{ width:420 }} onSubmit={onSubmit}>
      <h1>FleetTracker Admin</h1>
      <p style={{ color:'var(--text-secondary)' }}>Sign in to manage fleet operations</p>
      <div className="form-grid">
        <input placeholder="Username" value={username} onChange={(e)=>setUsername(e.target.value)} required />
        <input type="password" placeholder="Password" value={password} onChange={(e)=>setPassword(e.target.value)} required />
        {error && <small style={{ color:'var(--error)' }}>{error}</small>}
        <button className="button primary" disabled={loading}>{loading ? 'Signing in...' : 'Login'}</button>
      </div>
    </form>
  </div>;
}
