import axios from 'axios';
import { authStorage } from './authStorage';

export const api = axios.create({ baseURL: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000' });
api.interceptors.request.use((config) => {
  const token = authStorage.get();
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});
api.interceptors.response.use((r) => r, (error) => {
  if (error?.response?.status === 401) {
    authStorage.clear();
    window.location.href = '/login';
  }
  return Promise.reject(error);
});
