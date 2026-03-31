export type Id = string;
export interface Vehicle { id: Id; registrationPlate: string; make: string; model: string; year: number; payloadCapacityKg: number; hasRamp: boolean; initialMileageKm: number; initialMileageRecordedAtUtc: string; }
export interface Driver { id: Id; fullName: string; isActive: boolean; }
export interface Tour { id: Id; vehicleId: Id; vehicleRegistrationPlate: string; driverId: Id; driverFullName: string; date: string; tourNumber: number; unloadCount: number; weightKg: number; distanceKm: number; }
export interface AdminLoginResponse { accessToken: string; expiresAtUtc: string; username: string; role: string; }
export interface RegisterAdminRequest { username: string; password: string; confirmPassword: string; }
export interface RegisterAdminResponse { id: Id; username: string; role: string; isActive: boolean; createdAtUtc: string; }
export interface VehicleDailySummary { vehicleId: Id; registrationPlate: string; date: string; totalTours: number; totalUnloads: number; totalWeightKg: number; totalDistanceKm: number; distinctDriversCount: number; }
export interface VehicleMileage { vehicleId: Id; registrationPlate: string; initialMileageKm: number; tourDistanceTotalKm: number; currentMileageKm: number; }
