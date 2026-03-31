import 'package:flutter/material.dart';

import '../../core/services/local_storage_service.dart';
import '../../models/models.dart';
import '../../services/api_service.dart';

class DriverSelectionScreen extends StatefulWidget {
  const DriverSelectionScreen({super.key});

  @override
  State<DriverSelectionScreen> createState() => _DriverSelectionScreenState();
}

class _DriverSelectionScreenState extends State<DriverSelectionScreen> {
  late Future<List<DriverModel>> _future;

  @override
  void initState() {
    super.initState();
    _future = ApiService.instance.getDrivers();
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(title: const Text('Select Driver')),
        body: FutureBuilder<List<DriverModel>>(
          future: _future,
          builder: (_, snapshot) {
            if (snapshot.hasError) {
              return const Center(
                child: Text('Failed to load drivers. Please try again.'),
              );
            }

            if (!snapshot.hasData) {
              return const Center(child: CircularProgressIndicator());
            }

            final drivers = snapshot.data!;
            return ListView.builder(
              itemCount: drivers.length,
              itemBuilder: (_, i) => Card(
                child: ListTile(
                  title: Text(drivers[i].fullName),
                  onTap: () async {
                    await LocalStorageService.saveDriver(
                      drivers[i].id,
                      drivers[i].fullName,
                    );
                    if (!mounted) return;
                    Navigator.pushReplacementNamed(context, '/vehicle-selection');
                  },
                ),
              ),
            );
          },
        ),
      );
}
