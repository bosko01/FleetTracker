import 'package:dio/dio.dart';
import '../../models/models.dart';
import '../constants/app_config.dart';

class ApiService {
  ApiService._();
  static final ApiService instance = ApiService._();

  final Dio _dio = Dio(BaseOptions(baseUrl: AppConfig.apiBaseUrl));

  Future<List<DriverModel>> getDrivers() async {
    final res = await _dio.get('/api/mobile/drivers');
    return (res.data as List).map((e) => DriverModel.fromJson(e)).toList();
  }

  Future<List<VehicleModel>> getVehicles() async {
    final res = await _dio.get('/api/mobile/vehicles');
    return (res.data as List).map((e) => VehicleModel.fromJson(e)).toList();
  }

  Future<void> createTour({required String driverId, required String vehicleId, required String date, required int unloadCount, required double weightKg, required double distanceKm}) async {
    await _dio.post('/api/mobile/tours', data: {
      'driverId': driverId,
      'vehicleId': vehicleId,
      'date': date,
      'unloadCount': unloadCount,
      'weightKg': weightKg,
      'distanceKm': distanceKm,
    });
  }

  Future<List<MobileTourModel>> getTodayTours(String driverId) async {
    final date = DateTime.now().toUtc().toIso8601String().split('T').first;
    final res = await _dio.get('/api/mobile/drivers/$driverId/today-tours', queryParameters: { 'date': date });
    return (res.data as List).map((e) => MobileTourModel.fromJson(e)).toList();
  }
}
