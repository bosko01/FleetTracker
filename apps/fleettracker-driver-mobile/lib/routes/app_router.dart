import 'package:flutter/material.dart';
import '../features/driver_selection/driver_selection_screen.dart';
import '../features/home/splash_screen.dart';
import '../features/tours/new_tour_screen.dart';
import '../features/tours/today_tours_screen.dart';
import '../features/vehicle_selection/vehicle_selection_screen.dart';

class AppRouter {
  static const splash = '/';
  static final routes = <String, WidgetBuilder>{
    '/': (_) => const SplashScreen(),
    '/driver-selection': (_) => const DriverSelectionScreen(),
    '/vehicle-selection': (_) => const VehicleSelectionScreen(),
    '/new-tour': (_) => const NewTourScreen(),
    '/today-tours': (_) => const TodayToursScreen(),
  };
}
