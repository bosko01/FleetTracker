import { FormEvent, useMemo, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { driverApi, tourApi, vehicleApi } from '../services/api';
import { ConfirmDialog, DataTable, FormCard, PageHeader } from '../components/ui/Common';
import { EmptyState, ErrorState, LoadingState } from '../components/ui/State';

export function ToursListPage() {
  const queryClient = useQueryClient();
  const [filters, setFilters] = useState({ date: '', vehicleId: '', driverId: '' });
  const { data, isLoading, error } = useQuery({ queryKey: ['tours', filters], queryFn: () => tourApi.list(filters) });
  const drivers = useQuery({ queryKey: ['drivers'], queryFn: driverApi.list });
  const vehicles = useQuery({ queryKey: ['vehicles'], queryFn: vehicleApi.list });

  const totalDistance = useMemo(() => (data ?? []).reduce((sum, t) => sum + t.distanceKm, 0), [data]);

  if (isLoading || drivers.isLoading || vehicles.isLoading) return <LoadingState />;
  if (error || drivers.error || vehicles.error) return <ErrorState />;

  return <>
    <PageHeader title="Tours" description="Filter, edit, and delete any tour." />
    <div className="card form-inline">
      <input type="date" value={filters.date} onChange={(e) => setFilters({ ...filters, date: e.target.value })} />
      <select value={filters.vehicleId} onChange={(e) => setFilters({ ...filters, vehicleId: e.target.value })}>
        <option value="">All vehicles</option>
        {vehicles.data?.map((v) => <option key={v.id} value={v.id}>{v.registrationPlate}</option>)}
      </select>
      <select value={filters.driverId} onChange={(e) => setFilters({ ...filters, driverId: e.target.value })}>
        <option value="">All drivers</option>
        {drivers.data?.map((d) => <option key={d.id} value={d.id}>{d.fullName}</option>)}
      </select>
    </div>
    <div className="card">
      <p className="muted">TotalDistanceKm (filtered): {totalDistance.toFixed(2)}</p>
      {!data?.length ? <EmptyState label="No tours found." /> : <DataTable columns={['Date', 'Vehicle Registration', 'Driver Name', 'TourNumber', 'UnloadCount', 'WeightKg', 'DistanceKm', 'Actions']}>
        {data.map((t) => <tr key={t.id}>
          <td>{t.date}</td>
          <td>{t.vehicleRegistrationPlate}</td>
          <td>{t.driverFullName}</td>
          <td>{t.tourNumber}</td>
          <td>{t.unloadCount}</td>
          <td>{t.weightKg}</td>
          <td>{t.distanceKm}</td>
          <td className="actions-cell">
            <Link className="button secondary" to={`/tours/${t.id}/edit`}>Edit</Link>
            <ConfirmDialog title="Delete tour" message="This action permanently deletes the tour." confirmLabel="Delete" onConfirm={async () => {
              await tourApi.remove(t.id);
              await queryClient.invalidateQueries({ queryKey: ['tours'] });
            }} />
          </td>
        </tr>)}
      </DataTable>}
    </div>
  </>;
}

export function EditTourPage() {
  const nav = useNavigate();
  const { id } = useParams();
  const [form, setForm] = useState<Partial<{ vehicleId: string; driverId: string; date: string; unloadCount: number; weightKg: number; distanceKm: number }>>({});
  const [error, setError] = useState('');
  const tour = useQuery({ queryKey: ['tour', id], queryFn: () => tourApi.get(id!) });
  const drivers = useQuery({ queryKey: ['drivers'], queryFn: driverApi.list });
  const vehicles = useQuery({ queryKey: ['vehicles'], queryFn: vehicleApi.list });

  if (tour.isLoading || drivers.isLoading || vehicles.isLoading) return <LoadingState />;
  if (tour.error || drivers.error || vehicles.error) return <ErrorState />;

  const base = tour.data!;
  const value = {
    vehicleId: form.vehicleId ?? base.vehicleId,
    driverId: form.driverId ?? base.driverId,
    date: form.date ?? base.date,
    unloadCount: form.unloadCount ?? base.unloadCount,
    weightKg: form.weightKg ?? base.weightKg,
    distanceKm: form.distanceKm ?? base.distanceKm,
  };

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    if (!value.vehicleId || !value.driverId || !value.date || value.unloadCount < 0 || value.weightKg < 0 || value.distanceKm <= 0) {
      setError('Please fill all required fields. Validation: unload >= 0, weight >= 0, distance > 0.');
      return;
    }
    try {
      await tourApi.update(id!, value);
      nav('/tours');
    } catch {
      setError('Failed to update tour.');
    }
  };

  return <>
    <PageHeader title="Edit Tour" description="TourNumber is system controlled and read-only." />
    <form onSubmit={submit}>
      <FormCard title={`Tour #${base.tourNumber}`}>
        {error && <div className="error-text">{error}</div>}
        <label>TourNumber</label>
        <input value={base.tourNumber} readOnly />
        <label>Date</label>
        <input type="date" value={value.date} onChange={(e) => setForm({ ...form, date: e.target.value })} required />
        <label>Vehicle</label>
        <select value={value.vehicleId} onChange={(e) => setForm({ ...form, vehicleId: e.target.value })} required>
          {vehicles.data?.map((v) => <option key={v.id} value={v.id}>{v.registrationPlate}</option>)}
        </select>
        <label>Driver</label>
        <select value={value.driverId} onChange={(e) => setForm({ ...form, driverId: e.target.value })} required>
          {drivers.data?.map((d) => <option key={d.id} value={d.id}>{d.fullName}</option>)}
        </select>
        <input type="number" min={0} value={value.unloadCount} onChange={(e) => setForm({ ...form, unloadCount: Number(e.target.value) })} required />
        <input type="number" min={0} step="0.01" value={value.weightKg} onChange={(e) => setForm({ ...form, weightKg: Number(e.target.value) })} required />
        <input type="number" min={0.01} step="0.01" value={value.distanceKm} onChange={(e) => setForm({ ...form, distanceKm: Number(e.target.value) })} required />
        <button className="button primary">Save</button>
      </FormCard>
    </form>
  </>;
}
