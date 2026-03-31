import { Navigate } from 'react-router-dom';
import { authStorage } from '../services/authStorage';

export function ProtectedRoute({ children }: { children: JSX.Element }) {
  return authStorage.get() ? children : <Navigate to="/login" replace />;
}
