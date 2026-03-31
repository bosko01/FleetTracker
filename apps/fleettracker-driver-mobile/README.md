# FleetTracker Driver Mobile (Flutter)

## Run on Android Emulator
```bash
flutter pub get
flutter run --dart-define=API_BASE_URL=http://10.0.2.2:5000
```

## Run on Physical Android
```bash
flutter run --dart-define=API_BASE_URL=http://<LAN-IP>:5000
```

## Use ngrok backend URL
```bash
flutter run --dart-define=API_BASE_URL=https://<your-ngrok>.ngrok-free.app
```

## Build APK
```bash
flutter build apk --release --dart-define=API_BASE_URL=https://<your-api-host>
```
