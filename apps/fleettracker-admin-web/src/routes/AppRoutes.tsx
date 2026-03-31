import { Navigate, Route, Routes } from 'react-router-dom';
import { AdminLayout } from '../components/layout/AdminLayout';
import { ProtectedRoute } from './ProtectedRoute';
import { LoginPage } from '../pages/LoginPage';
import { RegisterPage } from '../pages/RegisterPage';
import { DashboardPage } from '../pages/DashboardPage';
import { CreateDriverPage, DriversListPage, EditDriverPage } from '../pages/DriversPages';
import { CreateVehiclePage, EditVehiclePage, VehiclesListPage } from '../pages/VehiclesPages';
import { EditTourPage, ToursListPage } from '../pages/ToursPages';
import { DailySummaryPage, StatisticsPage } from '../pages/StatisticsPages';

export function AppRoutes() {
  return <Routes>
    <Route path="/login" element={<LoginPage />} />
    <Route path="/register" element={<RegisterPage />} />
    <Route path="/" element={<ProtectedRoute><AdminLayout /></ProtectedRoute>}>
      <Route index element={<Navigate to="/dashboard" replace />} />
      <Route path="dashboard" element={<DashboardPage />} />
      <Route path="vehicles" element={<VehiclesListPage />} />
      <Route path="vehicles/new" element={<CreateVehiclePage />} />
      <Route path="vehicles/:id/edit" element={<EditVehiclePage />} />
      <Route path="drivers" element={<DriversListPage />} />
      <Route path="drivers/new" element={<CreateDriverPage />} />
      <Route path="drivers/:id/edit" element={<EditDriverPage />} />
      <Route path="tours" element={<ToursListPage />} />
      <Route path="tours/:id/edit" element={<EditTourPage />} />
      <Route path="statistics" element={<StatisticsPage />} />
      <Route path="daily-summary" element={<DailySummaryPage />} />
    </Route>
    <Route path="*" element={<Navigate to="/" replace />} />
  </Routes>;
}
