import { apiClient } from './apiClient';
import { AdminLoginResponse, Driver, RegisterAdminRequest, RegisterAdminResponse, Tour, Vehicle, VehicleDailySummary, VehicleMileage } from '../types/models';

export const authApi = {
  login: (username: string, password: string) => apiClient.post<AdminLoginResponse>('/api/admin/auth/login', { username, password }).then((r) => r.data),
  registerAdmin: (payload: RegisterAdminRequest) => apiClient.post<RegisterAdminResponse>('/api/admin/auth/register', payload).then((r) => r.data),
};
export const vehicleApi = {
  list: () => apiClient.get<Vehicle[]>('/api/vehicles').then((r) => r.data),
  get: (id: string) => apiClient.get<Vehicle>(`/api/vehicles/${id}`).then((r) => r.data),
  create: (payload: Omit<Vehicle, 'id' | 'initialMileageRecordedAtUtc'>) => apiClient.post('/api/vehicles', payload),
  update: (id: string, payload: Omit<Vehicle, 'id' | 'initialMileageRecordedAtUtc' | 'initialMileageKm'>) => apiClient.put(`/api/vehicles/${id}`, payload),
  remove: (id: string) => apiClient.delete(`/api/vehicles/${id}`),
};
export const driverApi = {
  list: () => apiClient.get<Driver[]>('/api/drivers').then((r) => r.data),
  get: (id: string) => apiClient.get<Driver>(`/api/drivers/${id}`).then((r) => r.data),
  create: (fullName: string) => apiClient.post('/api/drivers', { fullName }),
  update: (id: string, fullName: string) => apiClient.put(`/api/drivers/${id}`, { fullName }),
  activate: (id: string) => apiClient.patch(`/api/drivers/${id}/activate`),
  deactivate: (id: string) => apiClient.patch(`/api/drivers/${id}/deactivate`),
};
export const tourApi = {
  list: (params: { date?: string; vehicleId?: string; driverId?: string }) => apiClient.get<Tour[]>('/api/admin/tours', { params }).then((r) => r.data),
  get: (id: string) => apiClient.get<Tour>(`/api/tours/${id}`).then((r) => r.data),
  update: (id: string, payload: { vehicleId: string; driverId: string; date: string; unloadCount: number; weightKg: number; distanceKm: number }) => apiClient.put(`/api/tours/${id}`, payload),
  remove: (id: string) => apiClient.delete(`/api/tours/${id}`),
};
export const statsApi = {
  dailySummary: (vehicleId: string, date: string) => apiClient.get<VehicleDailySummary>(`/api/vehicles/${vehicleId}/daily-summary`, { params: { date } }).then((r) => r.data),
  totalMileage: (vehicleId: string) => apiClient.get<VehicleMileage>(`/api/vehicles/${vehicleId}/total-mileage`).then((r) => r.data),
};
