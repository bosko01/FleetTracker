import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter/foundation.dart';

import '../config/app_config.dart';
import '../models/models.dart';

class ApiService {
  ApiService._();
  static final ApiService instance = ApiService._();

  final Dio _dio = Dio(
    BaseOptions(
      baseUrl: AppConfig.apiBaseUrl,
      connectTimeout: const Duration(seconds: 15),
      receiveTimeout: const Duration(seconds: 20),
      sendTimeout: const Duration(seconds: 20),
    ),
  );

  Future<Response<dynamic>> get(
    String path, {
    Map<String, dynamic>? queryParameters,
  }) async {
    try {
      final response = await _dio.get(path, queryParameters: queryParameters);
      return response;
    } on DioException catch (error) {
      _logHttpError('GET', path, error);
      rethrow;
    } catch (error, stackTrace) {
      debugPrint('Unexpected GET error for $path: $error');
      log('Unexpected GET error for $path', error: error, stackTrace: stackTrace);
      rethrow;
    }
  }

  Future<Response<dynamic>> post(
    String path,
    Map<String, dynamic> body,
  ) async {
    try {
      final response = await _dio.post(path, data: body);
      return response;
    } on DioException catch (error) {
      _logHttpError('POST', path, error);
      rethrow;
    } catch (error, stackTrace) {
      debugPrint('Unexpected POST error for $path: $error');
      log('Unexpected POST error for $path', error: error, stackTrace: stackTrace);
      rethrow;
    }
  }

  Future<List<DriverModel>> getDrivers() async {
    final response = await get('/api/mobile/drivers');
    return (response.data as List)
        .map((entry) => DriverModel.fromJson(entry))
        .toList();
  }

  Future<List<VehicleModel>> getVehicles() async {
    final response = await get('/api/mobile/vehicles');
    return (response.data as List)
        .map((entry) => VehicleModel.fromJson(entry))
        .toList();
  }

  Future<void> createTour({
    required String driverId,
    required String vehicleId,
    required String date,
    required int unloadCount,
    required double weightKg,
    required double distanceKm,
  }) async {
    await post('/api/mobile/tours', {
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
    final response = await get(
      '/api/mobile/drivers/$driverId/today-tours',
      queryParameters: {'date': date},
    );

    return (response.data as List)
        .map((entry) => MobileTourModel.fromJson(entry))
        .toList();
  }

  void _logHttpError(String method, String path, DioException error) {
    final statusCode = error.response?.statusCode;
    final responseBody = error.response?.data;
    debugPrint(
      '$method $path failed | status: $statusCode | response: $responseBody',
    );
  }
}
