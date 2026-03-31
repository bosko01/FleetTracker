import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { statsApi, vehicleApi } from '../services/api';
import { PageHeader } from '../components/ui/Common';
import { LoadingState } from '../components/ui/State';

export function StatisticsPage() {
  const [vehicleId, setVehicleId] = useState('');
  const [date, setDate] = useState(new Date().toISOString().slice(0, 10));
  const vehicles = useQuery({ queryKey: ['vehicles'], queryFn: vehicleApi.list });
  const mileage = useQuery({ queryKey: ['mileage', vehicleId], queryFn: () => statsApi.totalMileage(vehicleId), enabled: !!vehicleId });
  const daily = useQuery({ queryKey: ['daily', vehicleId, date], queryFn: () => statsApi.dailySummary(vehicleId, date), enabled: !!vehicleId && !!date });

  if (vehicles.isLoading) return <LoadingState />;

  return <>
    <PageHeader title="Statistics" description="Vehicle-level daily summary and mileage analytics." />
    <div className="card form-inline">
      <select value={vehicleId} onChange={(e) => setVehicleId(e.target.value)}>
        <option value="">Choose vehicle</option>
        {vehicles.data?.map((v) => <option key={v.id} value={v.id}>{v.registrationPlate}</option>)}
      </select>
      <input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
    </div>

    <div className="grid-4" style={{ marginTop: '1rem' }}>
      <div className="card"><small>TourCount</small><h3>{daily.data?.totalTours ?? '-'}</h3></div>
      <div className="card"><small>TotalUnloadCount</small><h3>{daily.data?.totalUnloads ?? '-'}</h3></div>
      <div className="card"><small>TotalWeightKg</small><h3>{daily.data?.totalWeightKg ?? '-'}</h3></div>
      <div className="card"><small>TourDistanceTotalKm</small><h3>{mileage.data?.tourDistanceTotalKm ?? '-'}</h3></div>
    </div>

    <div className="grid-4" style={{ marginTop: '1rem' }}>
      <div className="card"><small>InitialMileageKm</small><h3>{mileage.data?.initialMileageKm ?? '-'}</h3></div>
      <div className="card"><small>CurrentMileageKm</small><h3>{mileage.data?.currentMileageKm ?? '-'}</h3></div>
      <div className="card"><small>Daily DistanceKm</small><h3>{daily.data?.totalDistanceKm ?? '-'}</h3></div>
      <div className="card"><small>DistinctDrivers</small><h3>{daily.data?.distinctDriversCount ?? '-'}</h3></div>
    </div>
  </>;
}

export const DailySummaryPage = StatisticsPage;
