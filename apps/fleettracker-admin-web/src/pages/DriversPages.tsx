import { FormEvent, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { driverApi } from '../services/api';
import { EmptyState, ErrorState, LoadingState } from '../components/ui/State';
import { DataTable, FormCard, PageHeader, StatusBadge } from '../components/ui/Common';

export function DriversListPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({ queryKey: ['drivers'], queryFn: driverApi.list });

  if (isLoading) return <LoadingState />;
  if (error) return <ErrorState />;
  if (!data?.length) return <EmptyState label="No drivers found." />;

  return <>
    <PageHeader title="Drivers" description="Manage all drivers and status." actionLabel="Add Driver" actionTo="/drivers/new" />
    <div className="card">
      <DataTable columns={['FullName', 'IsActive', 'Actions']}>
        {data.map((d) => <tr key={d.id}>
          <td>{d.fullName}</td>
          <td><StatusBadge active={d.isActive} /></td>
          <td className="actions-cell">
            <Link className="button secondary" to={`/drivers/${d.id}/edit`}>Edit</Link>
            {d.isActive
              ? <button className="button danger" onClick={async () => { await driverApi.deactivate(d.id); await queryClient.invalidateQueries({ queryKey: ['drivers'] }); }}>Deactivate</button>
              : <button className="button primary" onClick={async () => { await driverApi.activate(d.id); await queryClient.invalidateQueries({ queryKey: ['drivers'] }); }}>Activate</button>}
          </td>
        </tr>)}
      </DataTable>
    </div>
  </>;
}

function DriverForm({ mode }: { mode: 'create' | 'edit' }) {
  const nav = useNavigate();
  const { id } = useParams();
  const [fullName, setFullName] = useState('');
  const [error, setError] = useState('');
  const { data, isLoading, error: queryError } = useQuery({ queryKey: ['driver', id], queryFn: () => driverApi.get(id!), enabled: mode === 'edit' && !!id });

  if (isLoading) return <LoadingState />;
  if (queryError) return <ErrorState />;

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    const value = (fullName || data?.fullName || '').trim();
    if (!value) {
      setError('FullName is required.');
      return;
    }

    try {
      if (mode === 'create') await driverApi.create(value);
      else await driverApi.update(id!, value);
      nav('/drivers');
    } catch {
      setError('Unable to save driver.');
    }
  };

  return <>
    <PageHeader title={mode === 'create' ? 'Create Driver' : 'Edit Driver'} description="Create or update driver profile details." />
    <form onSubmit={submit}>
      <FormCard title="Driver">
        {error && <div className="error-text">{error}</div>}
        <input value={fullName || data?.fullName || ''} onChange={(e) => setFullName(e.target.value)} placeholder="FullName" required />
        {mode === 'edit' && <small className="muted">Status: {data?.isActive ? 'Active' : 'Inactive'}</small>}
        <button className="button primary">Save</button>
      </FormCard>
    </form>
  </>;
}

export const CreateDriverPage = () => <DriverForm mode="create" />;
export const EditDriverPage = () => <DriverForm mode="edit" />;
