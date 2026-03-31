import { FormEvent, useMemo, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { AxiosError } from 'axios';
import { authApi } from '../services/api';

type ValidationErrors = {
  username?: string;
  password?: string;
  confirmPassword?: string;
};

export function RegisterPage() {
  const navigate = useNavigate();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [errors, setErrors] = useState<ValidationErrors>({});
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const formHasValues = useMemo(() => username || password || confirmPassword, [username, password, confirmPassword]);

  const validate = () => {
    const nextErrors: ValidationErrors = {};
    if (!username.trim()) nextErrors.username = 'Username is required.';
    if (!password) nextErrors.password = 'Password is required.';
    else if (password.length < 8) nextErrors.password = 'Password must be at least 8 characters.';
    if (!confirmPassword) nextErrors.confirmPassword = 'Confirm password is required.';
    else if (password !== confirmPassword) nextErrors.confirmPassword = 'Passwords do not match.';
    setErrors(nextErrors);
    return Object.keys(nextErrors).length === 0;
  };

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (!validate()) return;

    setLoading(true);
    try {
      await authApi.registerAdmin({ username, password, confirmPassword });
      setSuccess('Admin account created successfully. Redirecting to sign in...');
      setTimeout(() => navigate('/login?registered=true'), 900);
    } catch (e) {
      const apiError = e as AxiosError<{ message?: string; errors?: string[] }>;
      if (apiError.response?.status === 409) {
        setError(apiError.response.data?.message ?? 'That username is already taken.');
      } else if (apiError.response?.status === 400) {
        const errorList = apiError.response.data?.errors;
        setError(errorList?.[0] ?? apiError.response.data?.message ?? 'Please correct the validation errors and try again.');
      } else {
        setError('Unable to register right now. Please try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  return <div style={{ minHeight: '100vh', display: 'grid', placeItems: 'center', background: 'var(--bg)', padding: '1rem' }}>
    <form className="card" style={{ width: 420 }} onSubmit={onSubmit}>
      <h1>FleetTracker Admin</h1>
      <p style={{ color: 'var(--text-secondary)', marginTop: 0 }}>Create your admin account</p>

      <div className="form-grid">
        <div>
          <input placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} maxLength={100} />
          {errors.username && <small style={{ color: 'var(--error)' }}>{errors.username}</small>}
        </div>

        <div>
          <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
          {errors.password && <small style={{ color: 'var(--error)' }}>{errors.password}</small>}
        </div>

        <div>
          <input type="password" placeholder="Confirm Password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />
          {errors.confirmPassword && <small style={{ color: 'var(--error)' }}>{errors.confirmPassword}</small>}
        </div>

        {error && <small style={{ color: 'var(--error)' }}>{error}</small>}
        {success && <small style={{ color: 'var(--success)' }}>{success}</small>}

        <button className="button primary" disabled={loading || !formHasValues}>{loading ? 'Creating account...' : 'Register'}</button>

        <small style={{ color: 'var(--text-secondary)', textAlign: 'center' }}>
          Already have an account? <Link to="/login" style={{ color: 'var(--primary)', fontWeight: 600 }}>Sign in</Link>
        </small>
      </div>
    </form>
  </div>;
}
