import { Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { driverApi, tourApi, vehicleApi } from '../services/api';
import { DataTable, PageHeader } from '../components/ui/Common';
import { EmptyState, LoadingState } from '../components/ui/State';

export function DashboardPage() {
  const today = new Date().toISOString().split('T')[0];
  const vehicles = useQuery({ queryKey: ['vehicles'], queryFn: vehicleApi.list });
  const drivers = useQuery({ queryKey: ['drivers'], queryFn: driverApi.list });
  const tours = useQuery({ queryKey: ['tours', today], queryFn: () => tourApi.list({ date: today }) });

  if (vehicles.isLoading || drivers.isLoading || tours.isLoading) return <LoadingState />;

  const totalDistanceToday = (tours.data ?? []).reduce((sum, t) => sum + t.distanceKm, 0);
  const cards = [
    ['Total vehicles', vehicles.data?.length ?? 0],
    ['Active drivers', drivers.data?.filter((d) => d.isActive).length ?? 0],
    ['Tours today', tours.data?.length ?? 0],
    ['Total distance today (km)', totalDistanceToday.toFixed(2)],
  ];

  return <>
    <PageHeader title="Dashboard" description="Admin overview and quick actions." />
    <div className="grid-4">{cards.map(([label, value]) => <div className="card" key={String(label)}><div className="muted">{label}</div><h2>{value}</h2></div>)}</div>
    <div className="card" style={{ marginTop: '1rem' }}>
      <h3>Quick actions</h3>
      <div className="actions-cell">
        <Link className="button primary" to="/vehicles/new">Add Vehicle</Link>
        <Link className="button secondary" to="/drivers/new">Add Driver</Link>
        <Link className="button secondary" to="/tours">View Tours</Link>
        <Link className="button secondary" to="/statistics">View Statistics</Link>
      </div>
    </div>
    <div className="card" style={{ marginTop: '1rem' }}>
      <h3>Recent tours</h3>
      {!tours.data?.length ? <EmptyState label="No tours found for today." /> : <DataTable columns={['Date', 'Vehicle', 'Driver', 'TourNumber', 'DistanceKm']}>
        {tours.data.slice(0, 8).map((t) => <tr key={t.id}><td>{t.date}</td><td>{t.vehicleRegistrationPlate}</td><td>{t.driverFullName}</td><td>{t.tourNumber}</td><td>{t.distanceKm}</td></tr>)}
      </DataTable>}
    </div>
  </>;
}
