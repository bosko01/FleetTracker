import { FormEvent, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useNavigate, useParams } from 'react-router-dom';
import { driverApi, tourApi, vehicleApi } from '../services/api';
import { EmptyState, LoadingState } from '../components/ui/State';

export function ToursListPage() {
  const queryClient = useQueryClient();
  const [filters, setFilters] = useState({ date:'', vehicleId:'', driverId:'' });
  const { data, isLoading } = useQuery({ queryKey:['tours',filters], queryFn:()=>tourApi.list(filters) });
  const drivers = useQuery({ queryKey:['drivers'], queryFn: driverApi.list });
  const vehicles = useQuery({ queryKey:['vehicles'], queryFn: vehicleApi.list });
  if (isLoading) return <LoadingState />;
  return <div className="card"><h3>Tours</h3>
    <div style={{display:'grid',gridTemplateColumns:'repeat(3,1fr)',gap:8,marginBottom:10}}>
      <input type="date" value={filters.date} onChange={e=>setFilters({...filters,date:e.target.value})} />
      <select value={filters.vehicleId} onChange={e=>setFilters({...filters,vehicleId:e.target.value})}><option value="">All vehicles</option>{vehicles.data?.map(v=><option key={v.id} value={v.id}>{v.registrationPlate}</option>)}</select>
      <select value={filters.driverId} onChange={e=>setFilters({...filters,driverId:e.target.value})}><option value="">All drivers</option>{drivers.data?.map(d=><option key={d.id} value={d.id}>{d.fullName}</option>)}</select>
    </div>
    {!data?.length ? <EmptyState label="No tours found" /> : <table className="table"><thead><tr><th>Date</th><th>Vehicle</th><th>Driver</th><th>Tour #</th><th>Unloads</th><th>Weight</th><th>Distance</th><th/></tr></thead><tbody>{data.map(t=><tr key={t.id}><td>{t.date}</td><td>{t.vehicleRegistrationPlate}</td><td>{t.driverFullName}</td><td>{t.tourNumber}</td><td>{t.unloadCount}</td><td>{t.weightKg}</td><td>{t.distanceKm}</td><td style={{display:'flex',gap:8}}><a className="button secondary" href={`/tours/${t.id}/edit`}>Edit</a><button className="button danger" onClick={async()=>{await tourApi.remove(t.id); queryClient.invalidateQueries({queryKey:['tours']});}}>Delete</button></td></tr>)}</tbody></table>}
  </div>;
}

export function EditTourPage() {
  const nav = useNavigate(); const { id } = useParams();
  const tour = useQuery({ queryKey:['tour',id], queryFn:()=>tourApi.get(id!) });
  const drivers = useQuery({ queryKey:['drivers'], queryFn: driverApi.list });
  const vehicles = useQuery({ queryKey:['vehicles'], queryFn: vehicleApi.list });
  const [form, setForm] = useState({ vehicleId:'', driverId:'', date:'', unloadCount:0, weightKg:0, distanceKm:0 });
  if (tour.isLoading) return <LoadingState />;
  const value = tour.data ? { vehicleId: tour.data.vehicleId, driverId: tour.data.driverId, date: tour.data.date, unloadCount: tour.data.unloadCount, weightKg: tour.data.weightKg, distanceKm: tour.data.distanceKm, ...form } : form;
  const submit = async (e: FormEvent) => { e.preventDefault(); await tourApi.update(id!, value); nav('/tours'); };
  return <form className="card form-grid" onSubmit={submit}><h3>Edit Tour #{tour.data?.tourNumber}</h3>
    <input type="date" value={value.date} onChange={e=>setForm({...form,date:e.target.value})} required />
    <select value={value.vehicleId} onChange={e=>setForm({...form,vehicleId:e.target.value})} required>{vehicles.data?.map(v=><option key={v.id} value={v.id}>{v.registrationPlate}</option>)}</select>
    <select value={value.driverId} onChange={e=>setForm({...form,driverId:e.target.value})} required>{drivers.data?.map(d=><option key={d.id} value={d.id}>{d.fullName}</option>)}</select>
    <input type="number" value={value.unloadCount} onChange={e=>setForm({...form,unloadCount:Number(e.target.value)})} required />
    <input type="number" value={value.weightKg} onChange={e=>setForm({...form,weightKg:Number(e.target.value)})} required />
    <input type="number" value={value.distanceKm} onChange={e=>setForm({...form,distanceKm:Number(e.target.value)})} required />
    <button className="button primary">Save</button>
  </form>;
}
