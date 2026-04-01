import 'dart:convert';
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
      headers: const {
        // Helps avoid ngrok browser warning HTML responses when using tunnel URLs.
        'ngrok-skip-browser-warning': 'true',
      },
    ),
  );

  Future<Response<dynamic>> get(
    String path, {
    Map<String, dynamic>? queryParameters,
  }) async {
    final requestUri = _buildRequestUri(path, queryParameters);

    try {
      debugPrint('HTTP GET -> $requestUri');
      final response = await _dio.get(path, queryParameters: queryParameters);
      debugPrint('HTTP GET <- ${response.statusCode} $requestUri');
      debugPrint('HTTP GET response body: ${_stringifyBody(response.data)}');
      return response;
    } on DioException catch (error) {
      _logHttpError('GET', path, error);
      rethrow;
    } catch (error, stackTrace) {
      debugPrint('Unexpected GET error for $requestUri: $error');
      log(
        'Unexpected GET error for $requestUri',
        error: error,
        stackTrace: stackTrace,
      );
      rethrow;
    }
  }

  Future<Response<dynamic>> post(
    String path,
    Map<String, dynamic> body,
  ) async {
    final requestUri = _buildRequestUri(path, null);

    try {
      debugPrint('HTTP POST -> $requestUri');
      debugPrint('HTTP POST request body: ${_stringifyBody(body)}');
      final response = await _dio.post(path, data: body);
      debugPrint('HTTP POST <- ${response.statusCode} $requestUri');
      debugPrint('HTTP POST response body: ${_stringifyBody(response.data)}');
      return response;
    } on DioException catch (error) {
      _logHttpError('POST', path, error);
      rethrow;
    } catch (error, stackTrace) {
      debugPrint('Unexpected POST error for $requestUri: $error');
      log(
        'Unexpected POST error for $requestUri',
        error: error,
        stackTrace: stackTrace,
      );
      rethrow;
    }
  }

  Future<List<DriverModel>> getDrivers() async {
    final response = await get('/api/mobile/drivers');

    try {
      final items = _extractList(response.data);
      return items
          .map((entry) => DriverModel.fromJson(Map<String, dynamic>.from(entry)))
          .toList();
    } catch (error, stackTrace) {
      _logParsingError('drivers', response.data, error, stackTrace);
      rethrow;
    }
  }

  Future<List<VehicleModel>> getVehicles() async {
    final response = await get('/api/mobile/vehicles');

    try {
      final items = _extractList(response.data);
      return items
          .map((entry) => VehicleModel.fromJson(Map<String, dynamic>.from(entry)))
          .toList();
    } catch (error, stackTrace) {
      _logParsingError('vehicles', response.data, error, stackTrace);
      rethrow;
    }
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

    try {
      final items = _extractList(response.data);
      return items
          .map(
            (entry) => MobileTourModel.fromJson(Map<String, dynamic>.from(entry)),
          )
          .toList();
    } catch (error, stackTrace) {
      _logParsingError('today tours', response.data, error, stackTrace);
      rethrow;
    }
  }

  void _logHttpError(String method, String path, DioException error) {
    final requestUri = _buildRequestUri(path, error.requestOptions.queryParameters);
    final statusCode = error.response?.statusCode;
    final responseBody = error.response?.data;

    debugPrint(
      '$method $requestUri failed | status: $statusCode | response: ${_stringifyBody(responseBody)}',
    );
  }

  Uri _buildRequestUri(String path, Map<String, dynamic>? queryParameters) {
    final normalizedPath = path.startsWith('/') ? path : '/$path';
    final base = Uri.parse(AppConfig.apiBaseUrl);

    return base.replace(
      path: normalizedPath,
      queryParameters: queryParameters?.isEmpty == true ? null : queryParameters,
    );
  }

  List<dynamic> _extractList(dynamic data) {
    if (data is List) return data;

    if (data is Map<String, dynamic>) {
      if (data['data'] is List) return data['data'] as List<dynamic>;
      if (data['items'] is List) return data['items'] as List<dynamic>;
    }

    throw FormatException('Expected JSON array but received ${data.runtimeType}');
  }

  String _stringifyBody(dynamic data) {
    if (data == null) return 'null';

    try {
      return const JsonEncoder.withIndent('  ').convert(data);
    } catch (_) {
      return data.toString();
    }
  }

  void _logParsingError(
    String resource,
    dynamic responseBody,
    Object error,
    StackTrace stackTrace,
  ) {
    debugPrint(
      'Failed to parse $resource response. Body: ${_stringifyBody(responseBody)}',
    );
    log(
      'Failed to parse $resource response',
      error: error,
      stackTrace: stackTrace,
    );
  }
}
