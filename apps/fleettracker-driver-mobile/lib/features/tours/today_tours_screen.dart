import 'package:flutter/material.dart';

import '../../core/services/local_storage_service.dart';
import '../../models/models.dart';
import '../../services/api_service.dart';

class TodayToursScreen extends StatefulWidget {
  const TodayToursScreen({super.key});

  @override
  State<TodayToursScreen> createState() => _TodayToursScreenState();
}

class _TodayToursScreenState extends State<TodayToursScreen> {
  Future<List<MobileTourModel>>? _future;

  @override
  void initState() {
    super.initState();
    LocalStorageService.readDriver().then((driver) {
      if (driver.$1 != null) {
        setState(() => _future = ApiService.instance.getTodayTours(driver.$1!));
      }
    });
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(title: const Text("Today's Tours")),
        body: _future == null
            ? const Center(child: Text('No driver selected'))
            : FutureBuilder<List<MobileTourModel>>(
                future: _future,
                builder: (_, snapshot) {
                  if (snapshot.hasError) {
                    return const Center(
                      child: Text('Failed to load tours. Please try again.'),
                    );
                  }

                  if (!snapshot.hasData) {
                    return const Center(child: CircularProgressIndicator());
                  }

                  final tours = snapshot.data!;
                  if (tours.isEmpty) {
                    return const Center(child: Text('No tours for today'));
                  }

                  return ListView.builder(
                    itemCount: tours.length,
                    itemBuilder: (_, i) {
                      final tour = tours[i];
                      return Card(
                        child: ListTile(
                          title: Text(
                            '${tour.registrationPlate} • Tour #${tour.tourNumber}',
                          ),
                          subtitle: Text(
                            'Unload: ${tour.unloadCount} | '
                            'Weight: ${tour.weightKg}kg | '
                            'Distance: ${tour.distanceKm}km',
                          ),
                        ),
                      );
                    },
                  );
                },
              ),
      );
}
