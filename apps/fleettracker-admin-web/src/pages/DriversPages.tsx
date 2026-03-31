import { FormEvent, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { driverApi } from '../services/api';
import { EmptyState, LoadingState } from '../components/ui/State';

export function DriversListPage() {
  const queryClient = useQueryClient();
  const { data, isLoading } = useQuery({ queryKey:['drivers'], queryFn: driverApi.list });
  if (isLoading) return <LoadingState />; if (!data?.length) return <EmptyState label="No drivers found" />;
  return <div className="card"><div style={{display:'flex',justifyContent:'space-between'}}><h3>Drivers</h3><Link className="button primary" to="/drivers/new">Add driver</Link></div>
  <table className="table"><thead><tr><th>Full name</th><th>Status</th><th/></tr></thead><tbody>{data.map(d=><tr key={d.id}><td>{d.fullName}</td><td>{d.isActive?'Active':'Inactive'}</td><td style={{display:'flex',gap:8}}><Link className="button secondary" to={`/drivers/${d.id}/edit`}>Edit</Link>{d.isActive?<button className="button danger" onClick={async()=>{await driverApi.deactivate(d.id);queryClient.invalidateQueries({queryKey:['drivers']});}}>Deactivate</button>:<button className="button primary" onClick={async()=>{await driverApi.activate(d.id);queryClient.invalidateQueries({queryKey:['drivers']});}}>Activate</button>}</td></tr>)}</tbody></table></div>;
}

function DriverForm({ mode }: { mode:'create'|'edit' }) {
  const nav = useNavigate(); const { id } = useParams();
  const { data } = useQuery({ queryKey:['driver',id], queryFn:()=>driverApi.get(id!), enabled: mode==='edit' && !!id });
  const [fullName, setFullName] = useState('');
  const submit = async (e: FormEvent) => { e.preventDefault(); const value = fullName || data?.fullName || ''; if (!value) return; if (mode === 'create') await driverApi.create(value); else await driverApi.update(id!, value); nav('/drivers'); };
  return <form className="card form-grid" onSubmit={submit}><h3>{mode==='create'?'Create':'Edit'} Driver</h3><input value={fullName || data?.fullName || ''} onChange={e=>setFullName(e.target.value)} placeholder="Full name" required /><button className="button primary">Save</button></form>;
}

export const CreateDriverPage = () => <DriverForm mode="create" />;
export const EditDriverPage = () => <DriverForm mode="edit" />;
