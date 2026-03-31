import { FormEvent, useMemo, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { vehicleApi } from '../services/api';
import { EmptyState, ErrorState, LoadingState } from '../components/ui/State';
import { ConfirmDialog, DataTable, FormCard, PageHeader } from '../components/ui/Common';

export function VehiclesListPage() {
  const queryClient = useQueryClient();
  const [search, setSearch] = useState('');
  const { data, isLoading, error } = useQuery({ queryKey: ['vehicles'], queryFn: vehicleApi.list });

  const filtered = useMemo(() => {
    if (!data) return [];
    const term = search.toLowerCase().trim();
    if (!term) return data;
    return data.filter((v) =>
      `${v.registrationPlate} ${v.make} ${v.model}`.toLowerCase().includes(term)
    );
  }, [data, search]);

  if (isLoading) return <LoadingState />;
  if (error) return <ErrorState />;

  return <>
    <PageHeader title="Vehicles" description="Create, edit, and deactivate vehicles." actionLabel="Add Vehicle" actionTo="/vehicles/new" />
    <div className="card">
      <input value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search by plate, make, or model" />
      {!filtered.length ? <EmptyState label="No vehicles found." /> : <DataTable columns={[
        'Registration Plate', 'Make', 'Model', 'Year', 'PayloadCapacityKg', 'HasRamp', 'InitialMileageKm', 'Actions',
      ]}>
        {filtered.map((v) => <tr key={v.id}>
          <td>{v.registrationPlate}</td>
          <td>{v.make}</td>
          <td>{v.model}</td>
          <td>{v.year}</td>
          <td>{v.payloadCapacityKg}</td>
          <td>{v.hasRamp ? 'Yes' : 'No'}</td>
          <td>{v.initialMileageKm}</td>
          <td className="actions-cell">
            <Link className="button secondary" to={`/vehicles/${v.id}/edit`}>Edit</Link>
            <ConfirmDialog title="Deactivate vehicle" message="This action will soft delete the vehicle." confirmLabel="Delete" onConfirm={async () => {
              await vehicleApi.remove(v.id);
              await queryClient.invalidateQueries({ queryKey: ['vehicles'] });
            }} />
          </td>
        </tr>)}
      </DataTable>}
    </div>
  </>;
}

function VehicleForm({ mode }: { mode: 'create' | 'edit' }) {
  const nav = useNavigate();
  const { id } = useParams();
  const isEdit = mode === 'edit';
  const existing = useQuery({ queryKey: ['vehicle', id], queryFn: () => vehicleApi.get(id!), enabled: isEdit && !!id });
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    registrationPlate: '',
    make: '',
    model: '',
    year: new Date().getFullYear(),
    payloadCapacityKg: 0,
    hasRamp: false,
    initialMileageKm: 0,
  });

  if (existing.isLoading) return <LoadingState />;
  if (existing.error) return <ErrorState />;

  const value = existing.data ?? form;
  const set = (k: string, v: string | number | boolean) => setForm((p) => ({ ...p, [k]: v }));

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');

    if (value.year < 1950 || value.payloadCapacityKg < 0 || value.initialMileageKm < 0) {
      setError('Please check Year, PayloadCapacityKg, and InitialMileageKm values.');
      return;
    }

    try {
      if (isEdit) {
        await vehicleApi.update(id!, value);
      } else {
        await vehicleApi.create(value);
      }
      nav('/vehicles');
    } catch {
      setError('Unable to save vehicle. Please verify data and try again.');
    }
  };

  return <>
    <PageHeader title={isEdit ? 'Edit Vehicle' : 'Create Vehicle'} description="Manage all editable vehicle fields." />
    <form onSubmit={submit}>
      <FormCard title={isEdit ? 'Vehicle Details' : 'New Vehicle'}>
        {error && <div className="error-text">{error}</div>}
        <input value={value.registrationPlate} onChange={(e) => set('registrationPlate', e.target.value)} placeholder="RegistrationPlate" required />
        <input value={value.make} onChange={(e) => set('make', e.target.value)} placeholder="Make" required />
        <input value={value.model} onChange={(e) => set('model', e.target.value)} placeholder="Model" required />
        <input type="number" min={1950} value={value.year} onChange={(e) => set('year', Number(e.target.value))} required />
        <input type="number" min={0} step="0.01" value={value.payloadCapacityKg} onChange={(e) => set('payloadCapacityKg', Number(e.target.value))} required />
        <input type="number" min={0} step="0.01" value={value.initialMileageKm} onChange={(e) => set('initialMileageKm', Number(e.target.value))} required />
        <label className="checkbox-label"><input type="checkbox" checked={value.hasRamp} onChange={(e) => set('hasRamp', e.target.checked)} /> HasRamp</label>
        {isEdit && <small className="muted">InitialMileageRecordedAtUtc: {existing.data?.initialMileageRecordedAtUtc ?? '-'}</small>}
        <button className="button primary">Save</button>
      </FormCard>
    </form>
  </>;
}

export const CreateVehiclePage = () => <VehicleForm mode="create" />;
export const EditVehiclePage = () => <VehicleForm mode="edit" />;
