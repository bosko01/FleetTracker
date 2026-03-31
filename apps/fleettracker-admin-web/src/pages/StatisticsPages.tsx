import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { statsApi, vehicleApi } from '../services/api';

export function StatisticsPage() {
  const [vehicleId, setVehicleId] = useState('');
  const [date, setDate] = useState(new Date().toISOString().slice(0,10));
  const vehicles = useQuery({ queryKey:['vehicles'], queryFn: vehicleApi.list });
  const mileage = useQuery({ queryKey:['mileage',vehicleId], queryFn:()=>statsApi.totalMileage(vehicleId), enabled: !!vehicleId });
  const daily = useQuery({ queryKey:['daily',vehicleId,date], queryFn:()=>statsApi.dailySummary(vehicleId, date), enabled: !!vehicleId && !!date });

  return <div className="card"><h3>Statistics by vehicle</h3>
    <div style={{display:'grid',gridTemplateColumns:'1fr 1fr', gap:8, marginBottom:10}}>
      <select value={vehicleId} onChange={e=>setVehicleId(e.target.value)}><option value="">Select vehicle</option>{vehicles.data?.map(v=><option key={v.id} value={v.id}>{v.registrationPlate}</option>)}</select>
      <input type="date" value={date} onChange={e=>setDate(e.target.value)} />
    </div>
    <div className="grid-4">
      <div className="card"><small>Current mileage</small><h3>{mileage.data?.currentMileageKm ?? '-'}</h3></div>
      <div className="card"><small>Tour distance total</small><h3>{mileage.data?.tourDistanceTotalKm ?? '-'}</h3></div>
      <div className="card"><small>Initial mileage</small><h3>{mileage.data?.initialMileageKm ?? '-'}</h3></div>
      <div className="card"><small>Total weight</small><h3>{daily.data?.totalWeightKg ?? '-'}</h3></div>
    </div>
  </div>;
}

export function DailySummaryPage() {
  const [vehicleId, setVehicleId] = useState('');
  const [date, setDate] = useState(new Date().toISOString().slice(0,10));
  const vehicles = useQuery({ queryKey:['vehicles'], queryFn: vehicleApi.list });
  const summary = useQuery({ queryKey:['daily-summary',vehicleId,date], queryFn:()=>statsApi.dailySummary(vehicleId,date), enabled: !!vehicleId });
  return <div className="card"><h3>Daily Summary</h3>
    <div style={{display:'grid',gridTemplateColumns:'1fr 1fr', gap:8}}>
      <select value={vehicleId} onChange={e=>setVehicleId(e.target.value)}><option value="">Select vehicle</option>{vehicles.data?.map(v=><option key={v.id} value={v.id}>{v.registrationPlate}</option>)}</select>
      <input type="date" value={date} onChange={e=>setDate(e.target.value)} />
    </div>
    {summary.data && <div style={{marginTop:12}}>
      <p>Registration: {summary.data.registrationPlate}</p>
      <p>Total distance: {summary.data.totalDistanceKm} km</p>
      <p>Tour count: {summary.data.totalTours}</p>
      <p>Total unload count: {summary.data.totalUnloads}</p>
      <p>Total weight: {summary.data.totalWeightKg} kg</p>
      <p>Distinct drivers: {summary.data.distinctDriversCount}</p>
    </div>}
  </div>;
}
