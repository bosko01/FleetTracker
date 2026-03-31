import 'package:flutter/material.dart';
import '../../core/services/local_storage_service.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});
  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen> {
  @override
  void initState() {
    super.initState();
    Future.delayed(const Duration(milliseconds: 250), () async {
      final saved = await LocalStorageService.readDriver();
      if (!mounted) return;
      Navigator.pushReplacementNamed(context, saved.$1 == null ? '/driver-selection' : '/vehicle-selection');
    });
  }

  @override
  Widget build(BuildContext context) => const Scaffold(body: Center(child: CircularProgressIndicator()));
}
