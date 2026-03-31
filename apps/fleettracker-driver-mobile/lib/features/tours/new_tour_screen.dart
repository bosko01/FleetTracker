import 'package:flutter/material.dart';
import '../../core/services/api_service.dart';
import '../../core/services/local_storage_service.dart';
import '../../models/models.dart';

class NewTourScreen extends StatefulWidget {
  const NewTourScreen({super.key});
  @override
  State<NewTourScreen> createState() => _NewTourScreenState();
}

class _NewTourScreenState extends State<NewTourScreen> {
  final _formKey = GlobalKey<FormState>();
  final _distance = TextEditingController();
  final _unloads = TextEditingController();
  final _weight = TextEditingController();
  String _driverId = '';
  String _driverName = '';

  @override
  void initState() { super.initState(); LocalStorageService.readDriver().then((v) => setState(() { _driverId = v.$1 ?? ''; _driverName = v.$2 ?? ''; })); }

  @override
  Widget build(BuildContext context) {
    final vehicle = ModalRoute.of(context)!.settings.arguments as VehicleModel;
    final date = DateTime.now().toIso8601String().split('T').first;
    return Scaffold(
      appBar: AppBar(title: const Text('New Tour Entry')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: ListView(children: [
            Text('Driver: $_driverName'),
            Text('Vehicle: ${vehicle.registrationPlate}'),
            Text('Date: $date'),
            const SizedBox(height: 12),
            TextFormField(controller: _distance, keyboardType: TextInputType.number, decoration: const InputDecoration(labelText: 'Distance (km)'), validator: (v)=> (v==null || v.isEmpty) ? 'Required' : null),
            const SizedBox(height: 8),
            TextFormField(controller: _unloads, keyboardType: TextInputType.number, decoration: const InputDecoration(labelText: 'Unload count'), validator: (v)=> (v==null || v.isEmpty) ? 'Required' : null),
            const SizedBox(height: 8),
            TextFormField(controller: _weight, keyboardType: TextInputType.number, decoration: const InputDecoration(labelText: 'Weight (kg)'), validator: (v)=> (v==null || v.isEmpty) ? 'Required' : null),
            const SizedBox(height: 14),
            ElevatedButton(onPressed: () async {
              if (!_formKey.currentState!.validate()) return;
              await ApiService.instance.createTour(driverId: _driverId, vehicleId: vehicle.id, date: date, unloadCount: int.parse(_unloads.text), weightKg: double.parse(_weight.text), distanceKm: double.parse(_distance.text));
              if (!mounted) return;
              ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('Tour saved successfully')));
              _distance.clear(); _unloads.clear(); _weight.clear();
            }, child: const Text('Save Tour')),
            const SizedBox(height: 8),
            OutlinedButton(onPressed: ()=>Navigator.pushReplacementNamed(context, '/vehicle-selection'), child: const Text('Change Vehicle')),
            const SizedBox(height: 8),
            OutlinedButton(onPressed: () async { await LocalStorageService.clearDriver(); if(!mounted) return; Navigator.pushReplacementNamed(context, '/driver-selection'); }, child: const Text('Change Driver')),
            const SizedBox(height: 8),
            ElevatedButton(onPressed: ()=> Navigator.pushNamed(context, '/today-tours'), child: const Text("Today's Tours")),
          ]),
        ),
      ),
    );
  }
}
