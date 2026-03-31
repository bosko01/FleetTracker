import { api } from './apiClient';
import { AdminLoginResponse, Driver, Tour, Vehicle, VehicleDailySummary, VehicleMileage } from '../types/models';

export const authApi = { login: (username: string, password: string) => api.post<AdminLoginResponse>('/api/admin/auth/login', { username, password }).then((r) => r.data) };
export const vehicleApi = {
  list: () => api.get<Vehicle[]>('/api/vehicles').then((r) => r.data),
  get: (id: string) => api.get<Vehicle>(`/api/vehicles/${id}`).then((r) => r.data),
  create: (payload: Omit<Vehicle, 'id'>) => api.post('/api/vehicles', payload),
  update: (id: string, payload: Omit<Vehicle, 'id'>) => api.put(`/api/vehicles/${id}`, payload),
  remove: (id: string) => api.delete(`/api/vehicles/${id}`),
};
export const driverApi = {
  list: () => api.get<Driver[]>('/api/drivers').then((r) => r.data),
  get: (id: string) => api.get<Driver>(`/api/drivers/${id}`).then((r) => r.data),
  create: (fullName: string) => api.post('/api/drivers', { fullName }),
  update: (id: string, fullName: string) => api.put(`/api/drivers/${id}`, { fullName }),
  activate: (id: string) => api.patch(`/api/drivers/${id}/activate`),
  deactivate: (id: string) => api.patch(`/api/drivers/${id}/deactivate`),
};
export const tourApi = {
  list: (params: { date?: string; vehicleId?: string; driverId?: string }) => api.get<Tour[]>('/api/admin/tours', { params }).then((r) => r.data),
  get: (id: string) => api.get<Tour>(`/api/tours/${id}`).then((r) => r.data),
  update: (id: string, payload: { vehicleId: string; driverId: string; date: string; unloadCount: number; weightKg: number; distanceKm: number }) => api.put(`/api/tours/${id}`, payload),
  remove: (id: string) => api.delete(`/api/tours/${id}`),
};
export const statsApi = {
  dailySummary: (vehicleId: string, date: string) => api.get<VehicleDailySummary>(`/api/vehicles/${vehicleId}/daily-summary`, { params: { date } }).then((r) => r.data),
  totalMileage: (vehicleId: string) => api.get<VehicleMileage>(`/api/vehicles/${vehicleId}/total-mileage`).then((r) => r.data),
};
