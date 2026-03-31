import 'package:flutter/material.dart';

import '../../models/models.dart';
import '../../services/api_service.dart';

class VehicleSelectionScreen extends StatefulWidget {
  const VehicleSelectionScreen({super.key});

  @override
  State<VehicleSelectionScreen> createState() => _VehicleSelectionScreenState();
}

class _VehicleSelectionScreenState extends State<VehicleSelectionScreen> {
  late Future<List<VehicleModel>> _future;

  @override
  void initState() {
    super.initState();
    _future = ApiService.instance.getVehicles();
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(title: const Text('Select Vehicle')),
        body: FutureBuilder<List<VehicleModel>>(
          future: _future,
          builder: (_, snapshot) {
            if (snapshot.hasError) {
              return const Center(
                child: Text('Failed to load vehicles. Please try again.'),
              );
            }

            if (!snapshot.hasData) {
              return const Center(child: CircularProgressIndicator());
            }

            final vehicles = snapshot.data!;
            return ListView.builder(
              itemCount: vehicles.length,
              itemBuilder: (_, i) {
                final vehicle = vehicles[i];
                return Card(
                  child: ListTile(
                    title: Text(vehicle.registrationPlate),
                    subtitle: Text(
                      '${vehicle.make} ${vehicle.model}${vehicle.hasRamp ? ' • Ramp' : ''}',
                    ),
                    onTap: () => Navigator.pushReplacementNamed(
                      context,
                      '/new-tour',
                      arguments: vehicle,
                    ),
                  ),
                );
              },
            );
          },
        ),
      );
}
