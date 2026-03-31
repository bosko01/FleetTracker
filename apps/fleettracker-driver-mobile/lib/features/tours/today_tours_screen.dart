import 'package:flutter/material.dart';
import '../../core/services/api_service.dart';
import '../../core/services/local_storage_service.dart';
import '../../models/models.dart';

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
    LocalStorageService.readDriver().then((v) {
      if (v.$1 != null) setState(() => _future = ApiService.instance.getTodayTours(v.$1!));
    });
  }

  @override
  Widget build(BuildContext context) => Scaffold(
    appBar: AppBar(title: const Text("Today's Tours")),
    body: _future == null ? const Center(child: Text('No driver selected')) : FutureBuilder<List<MobileTourModel>>(
      future: _future,
      builder: (_, snapshot) {
        if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
        final tours = snapshot.data!;
        if (tours.isEmpty) return const Center(child: Text('No tours for today'));
        return ListView.builder(itemCount: tours.length, itemBuilder: (_, i) {
          final t = tours[i];
          return Card(child: ListTile(title: Text('${t.registrationPlate} • Tour #${t.tourNumber}'), subtitle: Text('Unload: ${t.unloadCount} | Weight: ${t.weightKg}kg | Distance: ${t.distanceKm}km')));
        });
      },
    ),
  );
}
