import 'package:flutter/material.dart';
import '../core/theme/app_theme.dart';
import '../routes/app_router.dart';

class FleetTrackerDriverApp extends StatelessWidget {
  const FleetTrackerDriverApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'FleetTracker Driver',
      debugShowCheckedModeBanner: false,
      theme: AppTheme.theme,
      initialRoute: AppRouter.splash,
      routes: AppRouter.routes,
    );
  }
}
