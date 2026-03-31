import { Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { driverApi, tourApi, vehicleApi } from '../services/api';

export function DashboardPage() {
  const vehicles = useQuery({ queryKey:['vehicles'], queryFn: vehicleApi.list });
  const drivers = useQuery({ queryKey:['drivers'], queryFn: driverApi.list });
  const today = new Date().toISOString().split('T')[0];
  const tours = useQuery({ queryKey:['tours',today], queryFn:()=>tourApi.list({ date: today }) });

  const cards = [
    ['Total vehicles', vehicles.data?.length ?? 0],
    ['Active drivers', drivers.data?.filter(d=>d.isActive).length ?? 0],
    ['Tours today', tours.data?.length ?? 0],
  ];

  return <>
    <div className="grid-4">{cards.map(([label,value]) => <div className="card" key={String(label)}><div>{label}</div><h2>{value}</h2></div>)}</div>
    <div className="card" style={{ marginTop:'1rem' }}>
      <h3>Quick actions</h3>
      <div style={{ display:'flex', gap:'.7rem' }}>
        <Link className="button primary" to="/vehicles/new">Create Vehicle</Link>
        <Link className="button secondary" to="/drivers/new">Create Driver</Link>
        <Link className="button secondary" to="/tours">Review Tours</Link>
      </div>
    </div>
  </>;
}
