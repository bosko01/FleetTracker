import { FormEvent, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { vehicleApi } from '../services/api';
import { EmptyState, ErrorState, LoadingState } from '../components/ui/State';

export function VehiclesListPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({ queryKey:['vehicles'], queryFn: vehicleApi.list });
  if (isLoading) return <LoadingState />; if (error) return <ErrorState />; if (!data?.length) return <EmptyState label="No vehicles found" />;
  return <div className="card"><div style={{display:'flex',justifyContent:'space-between'}}><h3>Vehicles</h3><Link className="button primary" to="/vehicles/new">Add vehicle</Link></div>
  <table className="table"><thead><tr><th>Plate</th><th>Make</th><th>Model</th><th>Year</th><th>Payload</th><th>Initial mileage (km)</th><th>Ramp</th><th/></tr></thead><tbody>
  {data.map(v=><tr key={v.id}><td>{v.registrationPlate}</td><td>{v.make}</td><td>{v.model}</td><td>{v.year}</td><td>{v.payloadCapacityKg}</td><td>{v.initialMileageKm}</td><td>{v.hasRamp?'Yes':'No'}</td><td style={{display:'flex',gap:8}}><Link className="button secondary" to={`/vehicles/${v.id}/edit`}>Edit</Link><button className="button danger" onClick={async()=>{await vehicleApi.remove(v.id); queryClient.invalidateQueries({queryKey:['vehicles']});}}>Delete</button></td></tr>)}
  </tbody></table></div>;
}

function VehicleForm({ mode }: { mode:'create'|'edit' }) {
  const nav = useNavigate(); const { id } = useParams();
  const existing = useQuery({ queryKey:['vehicle',id], queryFn:()=>vehicleApi.get(id!), enabled: mode==='edit' && !!id });
  const [form, setForm] = useState({ registrationPlate:'', make:'', model:'', year: new Date().getFullYear(), payloadCapacityKg:0, hasRamp:false, initialMileageKm:0 });
  const value = existing.data ?? form;
  const set = (k: string, v: string|number|boolean) => setForm((p)=>({ ...p,[k]: v }));
  const submit = async (e: FormEvent)=>{ e.preventDefault(); if(mode==='create'){ await vehicleApi.create(form); } else { const updatePayload = { registrationPlate: value.registrationPlate, make: value.make, model: value.model, year: value.year, payloadCapacityKg: value.payloadCapacityKg, hasRamp: value.hasRamp }; await vehicleApi.update(id!, updatePayload); } nav('/vehicles'); };
  if (existing.isLoading) return <LoadingState />;
  return <form className="card form-grid" onSubmit={submit}><h3>{mode==='create'?'Create':'Edit'} Vehicle</h3>
    <input value={value.registrationPlate} onChange={e=>set('registrationPlate',e.target.value)} placeholder="Registration plate" required />
    <input value={value.make} onChange={e=>set('make',e.target.value)} placeholder="Make" required />
    <input value={value.model} onChange={e=>set('model',e.target.value)} placeholder="Model" required />
    <input type="number" value={value.year} onChange={e=>set('year',Number(e.target.value))} required />
    <input type="number" min={0} value={value.payloadCapacityKg} onChange={e=>set('payloadCapacityKg',Number(e.target.value))} required />
    {mode==='create' ? (
      <label>
        Initial Mileage (km)
        <input type="number" min={0} value={value.initialMileageKm} onChange={e=>set('initialMileageKm',Number(e.target.value))} required />
        <small>Enter the vehicle's current odometer value in kilometers.</small>
      </label>
    ) : (
      <label>
        Initial Mileage (km)
        <input type="number" value={value.initialMileageKm} readOnly />
        <small>Set when the vehicle was created.</small>
      </label>
    )}
    <label><input type="checkbox" checked={value.hasRamp} onChange={e=>set('hasRamp',e.target.checked)} /> Has ramp</label>
    <button className="button primary">Save</button>
  </form>;
}

export const CreateVehiclePage = () => <VehicleForm mode="create" />;
export const EditVehiclePage = () => <VehicleForm mode="edit" />;
