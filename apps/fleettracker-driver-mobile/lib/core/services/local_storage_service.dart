import 'package:shared_preferences/shared_preferences.dart';

class LocalStorageService {
  static const _driverId = 'selected_driver_id';
  static const _driverName = 'selected_driver_name';

  static Future<void> saveDriver(String id, String name) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_driverId, id);
    await prefs.setString(_driverName, name);
  }

  static Future<(String?, String?)> readDriver() async {
    final prefs = await SharedPreferences.getInstance();
    return (prefs.getString(_driverId), prefs.getString(_driverName));
  }

  static Future<void> clearDriver() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove(_driverId);
    await prefs.remove(_driverName);
  }
}
