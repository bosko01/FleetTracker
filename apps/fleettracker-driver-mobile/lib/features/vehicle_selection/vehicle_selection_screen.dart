import 'package:flutter/material.dart';
import '../../core/services/api_service.dart';
import '../../models/models.dart';

class VehicleSelectionScreen extends StatefulWidget {
  const VehicleSelectionScreen({super.key});
  @override
  State<VehicleSelectionScreen> createState() => _VehicleSelectionScreenState();
}

class _VehicleSelectionScreenState extends State<VehicleSelectionScreen> {
  late Future<List<VehicleModel>> _future;
  @override
  void initState() { super.initState(); _future = ApiService.instance.getVehicles(); }

  @override
  Widget build(BuildContext context) => Scaffold(
    appBar: AppBar(title: const Text('Select Vehicle')),
    body: FutureBuilder<List<VehicleModel>>(
      future: _future,
      builder: (_, snapshot) {
        if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
        final vehicles = snapshot.data!;
        return ListView.builder(
          itemCount: vehicles.length,
          itemBuilder: (_, i) {
            final v = vehicles[i];
            return Card(child: ListTile(
              title: Text(v.registrationPlate),
              subtitle: Text('${v.make} ${v.model}${v.hasRamp ? ' • Ramp' : ''}'),
              onTap: () => Navigator.pushReplacementNamed(context, '/new-tour', arguments: v),
            ));
          },
        );
      },
    ),
  );
}
