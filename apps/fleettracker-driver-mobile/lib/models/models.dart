class DriverModel {
  final String id;
  final String fullName;
  DriverModel({required this.id, required this.fullName});
  factory DriverModel.fromJson(Map<String, dynamic> json) => DriverModel(id: json['id'], fullName: json['fullName']);
}

class VehicleModel {
  final String id;
  final String registrationPlate;
  final String make;
  final String model;
  final bool hasRamp;
  VehicleModel({required this.id, required this.registrationPlate, required this.make, required this.model, required this.hasRamp});
  factory VehicleModel.fromJson(Map<String, dynamic> json) => VehicleModel(id: json['id'], registrationPlate: json['registrationPlate'], make: json['make'], model: json['model'], hasRamp: json['hasRamp']);
}

class MobileTourModel {
  final String id;
  final String date;
  final String registrationPlate;
  final int tourNumber;
  final int unloadCount;
  final num weightKg;
  final num distanceKm;
  MobileTourModel({required this.id, required this.date, required this.registrationPlate, required this.tourNumber, required this.unloadCount, required this.weightKg, required this.distanceKm});
  factory MobileTourModel.fromJson(Map<String, dynamic> json) => MobileTourModel(id: json['id'], date: json['date'], registrationPlate: json['registrationPlate'], tourNumber: json['tourNumber'], unloadCount: json['unloadCount'], weightKg: json['weightKg'], distanceKm: json['distanceKm']);
}
