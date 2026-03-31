# FleetTracker — LLM-Optimizovana Tehnička Dokumentacija (Kompletan Projekat)

> Svrha ovog dokumenta: da omogući i čoveku i LLM-u brzo i pouzdano razumevanje arhitekture, pravila domena, API ugovora, tokova podataka i ključnih odluka u projektu.

---

## 1) Projekat u jednoj rečenici

**FleetTracker** je .NET 8 Web API za evidenciju vozila i njihovih tura, sa Clean Architecture + CQRS pristupom, soft-delete logikom za vozila i izračunom statistike (dnevni zbir i ukupna kilometraža) na osnovu tabela tura.

---

## 2) Tehnologije i osnovne zavisnosti

- **Platforma:** .NET 8 (`net8.0`)
- **API:** ASP.NET Core Web API + Swagger (u development okruženju)
- **Arhitektura aplikacije:** Clean Architecture (API / Application / Domain / Infrastructure)
- **CQRS + Mediator:** MediatR
- **Validacija:** FluentValidation + MediatR pipeline behavior
- **Perzistencija:** Entity Framework Core 8 + SQL Server provider
- **Greške:** centralizovani global exception middleware

---

## 3) Struktura solution-a i odgovornosti slojeva

- `src/FleetTracker.API`
  - HTTP ulazna tačka (kontroleri)
  - mapiranje request DTO -> Command/Query
  - middleware za globalno rukovanje greškama
  - DI bootstrap + swagger + runtime konfiguracija

- `src/FleetTracker.Application`
  - use-case logika kroz **Commands/Queries + Handlers**
  - validacija komandi kroz FluentValidation
  - interfejsi repozitorijuma (`IVehicleRepository`, `ITourRepository`) i `IUnitOfWork`
  - aplikativni exception tipovi

- `src/FleetTracker.Domain`
  - poslovni model i pravila (entiteti `Vehicle`, `Tour`)
  - domain validacije i domain exception
  - bazne klase (`BaseEntity`, `SoftDeletableEntity`)

- `src/FleetTracker.Infrastructure`
  - EF Core DbContext i konfiguracije entiteta
  - implementacije repozitorijuma i Unit of Work
  - registracija DB + infrastrukture u DI

---

## 4) Brzi mentalni model sistema (LLM-friendly)

Ako LLM treba da "razmišlja" o ovom kodu, koristi sledeći model:

1. **Sve spoljne operacije dolaze preko API kontrolera**.
2. Kontroler šalje tačno jedan **MediatR Command/Query**.
3. Pre Handler-a radi se **FluentValidation** (pipeline).
4. Handler koristi **repository interfejse** i eventualno `IUnitOfWork.SaveChangesAsync()`.
5. Entiteti domena (`Vehicle`, `Tour`) sprovode centralna domain pravila.
6. Sve izuzetke obrađuje global middleware i prevodi u HTTP status + `ErrorResponse`.

---

## 5) Domen: entiteti, atributi i pravila

### 5.1 Vehicle

**Polja:**
- `Id : Guid`
- `RegistrationPlate : string` (normalizuje se na `Trim + UpperInvariant`)
- `Make : string`
- `Model : string`
- `Year : int` (1950 .. trenutna godina + 1)
- `PayloadCapacityKg : decimal` (>= 0)
- `HasRamp : bool`
- `IsDeleted : bool` (soft delete)
- `CreatedAtUtc`, `ModifiedAtUtc`

**Ponašanja:**
- `Create(...)` — kreira novi vozilo uz validaciju
- `UpdateDetails(...)` — menja podatke uz validaciju
- `SoftDelete()` — postavlja `IsDeleted = true`

**Pravila:**
- registracija/make/model ne smeju biti prazni
- godina mora biti u granicama
- nosivost ne sme biti negativna

---

### 5.2 Tour

**Polja:**
- `Id : Guid`
- `VehicleId : Guid`
- `Date : DateOnly`
- `TourNumber : int` (> 0)
- `UnloadCount : int` (>= 0)
- `WeightKg : decimal` (>= 0)
- `DistanceKm : decimal` (> 0)
- `CreatedAtUtc`, `ModifiedAtUtc`

**Ponašanja:**
- `Create(...)`
- `Update(...)`

**Pravila:**
- `VehicleId` ne sme biti prazan Guid
- `TourNumber > 0`
- `UnloadCount >= 0`
- `WeightKg >= 0`
- `DistanceKm > 0`

---

## 6) Baza podataka i EF Core mapiranje

### 6.1 Tabele
- `Vehicles`
- `Tours`

### 6.2 Ključni indeksi i ograničenja
- `Vehicles.RegistrationPlate` — **UNIQUE**
- `Tours (VehicleId, Date, TourNumber)` — **UNIQUE**

### 6.3 Relacije
- `Vehicle 1 -- * Tour`
- `OnDelete(DeleteBehavior.Restrict)` između `Vehicle` i `Tour`

### 6.4 Soft delete
- Global query filter za `Vehicle`: vraća samo `!IsDeleted`.
- Repository ponekad koristi `IgnoreQueryFilters()` kada je potrebno eksplicitno proveriti i obrisane zapise.

---

## 7) API endpoint-i (kompletan pregled)

## Vozila

### `POST /api/vehicles`
Kreira vozilo.

**Body:**
```json
{
  "registrationPlate": "BG-123-AB",
  "make": "MAN",
  "model": "TGS",
  "year": 2023,
  "payloadCapacityKg": 12000,
  "hasRamp": true
}
```

**Uspeh:** `201 Created` + `VehicleResponse`

---

### `GET /api/vehicles`
Vraća listu aktivnih (ne-soft-deleted) vozila.

**Uspeh:** `200 OK` + `VehicleListItemResponse[]`

---

### `GET /api/vehicles/{id}`
Vraća vozilo po ID-u.

**Uspeh:** `200 OK` + `VehicleResponse`

**Napomena:** trenutno može vratiti i soft-deleted vozilo (jer handler ne proverava `IsDeleted` za ovaj slučaj).

---

### `PUT /api/vehicles/{id}`
Ažurira vozilo.

**Uspeh:** `200 OK` + `VehicleResponse`

**Greške:**
- `404` ako vozilo ne postoji
- `409` ako je registracija zauzeta
- `400` za validation/domain greške

---

### `DELETE /api/vehicles/{id}`
Soft delete vozila.

**Uspeh:** `204 No Content`

---

## Ture

### `POST /api/vehicles/{vehicleId}/tours`
Kreira turu za vozilo.

**Uspeh:** `201 Created` + `TourResponse`

**Greške:**
- `404` ako vozilo ne postoji ili je soft-deleted
- `409` ako već postoji isti `(vehicleId, date, tourNumber)`

---

### `GET /api/vehicles/{vehicleId}/tours?date=YYYY-MM-DD`
- bez `date`: vraća sve ture za vozilo
- sa `date`: vraća ture samo za taj datum

**Uspeh:** `200 OK` + `TourListItemResponse[]`

---

### `GET /api/tours/{id}`
Vraća turu po ID-u.

**Uspeh:** `200 OK` + `TourResponse`

---

### `PUT /api/tours/{id}`
Ažurira postojeću turu.

**Uspeh:** `200 OK` + `TourResponse`

---

### `DELETE /api/tours/{id}`
Fizički briše turu.

**Uspeh:** `204 No Content`

---

## Statistika

### `GET /api/vehicles/{vehicleId}/daily-summary?date=YYYY-MM-DD`
Računa zbirne vrednosti za dan:
- broj tura
- ukupan broj istovara
- ukupna težina
- ukupna kilometraža

**Uspeh:** `200 OK` + `VehicleDailySummaryResponse`

---

### `GET /api/vehicles/{vehicleId}/total-mileage`
Računa ukupnu kilometražu vozila kao sumu `Tour.DistanceKm`.

**Uspeh:** `200 OK` + `VehicleTotalMileageResponse`

---

## 8) CQRS katalog (šta je command/query i šta radi)

### Vehicle Commands
- `CreateVehicleCommand` — kreira vozilo (provera jedinstvene registracije)
- `UpdateVehicleCommand` — menja vozilo (provera jedinstvene registracije)
- `DeleteVehicleCommand` — soft delete

### Vehicle Queries
- `GetAllVehiclesQuery` — lista vozila
- `GetVehicleByIdQuery` — detalj vozila

### Tour Commands
- `CreateTourCommand` — kreira turu (provera jedinstvenosti po vozilo+datum+broj ture)
- `UpdateTourCommand` — menja turu (ista provera jedinstvenosti)
- `DeleteTourCommand` — fizičko brisanje ture

### Tour Queries
- `GetTourByIdQuery`
- `GetToursByVehicleQuery`
- `GetToursByVehicleAndDateQuery`

### Statistics Queries
- `GetVehicleDailySummaryQuery`
- `GetVehicleTotalMileageQuery`

---

## 9) Validacija i greške

### 9.1 Dva nivoa validacije
1. **Application validacija** (FluentValidation) preko MediatR pipeline-a.
2. **Domain validacija** unutar entiteta (`Vehicle`, `Tour`).

Ovo znači da i ako se handler pozove bez API sloja, domain i dalje čuva ključna pravila.

### 9.2 Mapa exception -> HTTP status
- `NotFoundException` -> `404`
- `ConflictException` -> `409`
- `DomainRuleViolationException` -> `400`
- `FluentValidation.ValidationException` -> `400` + lista grešaka
- ostalo -> `500`

### 9.3 Standardni format greške
```json
{
  "message": "Validation failed.",
  "errors": ["..."]
}
```

---

## 10) Tokovi podataka (end-to-end primeri)

### 10.1 Kreiranje vozila
HTTP `POST /api/vehicles` -> `CreateVehicleCommand` -> validator -> handler ->
provera duplikata registracije -> `Vehicle.Create(...)` -> repository `AddAsync` -> `UnitOfWork.SaveChangesAsync` -> response.

### 10.2 Kreiranje ture
HTTP `POST /api/vehicles/{vehicleId}/tours` -> `CreateTourCommand` -> validator -> handler ->
provera da vozilo postoji i nije obrisano -> provera duplikata ture za datum -> `Tour.Create(...)` -> save -> response.

### 10.3 Dnevna statistika
HTTP `GET /api/vehicles/{vehicleId}/daily-summary?date=...` -> query handler ->
provera vozila -> čitanje tura za datum -> agregacija (`Count`, `Sum`) -> response.

---

## 11) Ključna poslovna pravila i implikacije

1. **Registracija vozila je globalno jedinstvena** (uključujući soft-deleted vozila, jer se provera radi sa `IgnoreQueryFilters`).
2. **Vozilo se ne briše fizički**, već soft-delete.
3. **Tura se briše fizički** (`Remove`).
4. **Statistika ignoriše soft-deleted vozila** (za takva vozila vraća 404 pre računanja).
5. **Jedinstvenost ture** je po `(VehicleId, Date, TourNumber)`.

---

## 12) Potencijalne nedoslednosti / stvari koje LLM treba da zna

- `GetVehicleByIdQueryHandler` ne proverava `IsDeleted`, pa može vratiti i soft-deleted vozilo.
- `DeleteVehicleCommandHandler` ne proverava da li je vozilo već obrisano (operacija je praktično idempotentna na nivou posledice).
- `VehicleRepository.GetByRegistrationPlateAsync` koristi `ToUpper()` (ne `ToUpperInvariant()`), dok ostatak uglavnom koristi invariant normalizaciju.

Ovo nije nužno bug, ali su bitni detalji za precizno rezonovanje.

---

## 13) Pokretanje projekta (lokalno)

### Preduslovi
- .NET SDK 8.x
- SQL Server instanca dostupna prema connection string-u

### Koraci
1. Podesiti `DefaultConnection` u `src/FleetTracker.API/appsettings.json`.
2. Kreirati migraciju (po potrebi):
   ```bash
   dotnet ef migrations add InitialCreate --project src/FleetTracker.Infrastructure --startup-project src/FleetTracker.API
   ```
3. Primeni migracije:
   ```bash
   dotnet ef database update --project src/FleetTracker.Infrastructure --startup-project src/FleetTracker.API
   ```
4. Pokreni API:
   ```bash
   dotnet run --project src/FleetTracker.API
   ```

Swagger UI je dostupan u development režimu.

---

## 14) Predlog za LLM upotrebu (prompting kontekst)

Ako LLM-u daješ ovaj projekat, preporučen minimalni kontekst je:
1. Ovaj dokument (`LLM_PROJECT_DOCUMENTATION.md`)
2. `Program.cs` + svi kontroleri
3. Domain entiteti (`Vehicle`, `Tour`)
4. Svi handler-i za feature koji se menja
5. Repo + EF konfiguracije tog entiteta

**Za izmene pravila validacije**: menjati i FluentValidation i Domain validaciju (da ostanu usklađeni).

---

## 15) Datoteke visokog prioriteta za razumevanje

1. `src/FleetTracker.API/Program.cs`
2. `src/FleetTracker.API/Controllers/*.cs`
3. `src/FleetTracker.API/Middleware/GlobalExceptionHandlingMiddleware.cs`
4. `src/FleetTracker.Domain/Entities/Vehicle.cs`
5. `src/FleetTracker.Domain/Entities/Tour.cs`
6. `src/FleetTracker.Application/Features/**/` (command/query handleri)
7. `src/FleetTracker.Infrastructure/Persistence/Configurations/*.cs`
8. `src/FleetTracker.Infrastructure/Persistence/Repositories/*.cs`

---

## 16) Kratki "ontology" blok za mašinsko parsiranje

```yaml
project:
  name: FleetTracker
  architecture: CleanArchitecture+CQRS
  runtime: net8.0
  db: SQLServer
entities:
  Vehicle:
    soft_delete: true
    unique: [RegistrationPlate]
    fields:
      - Id: Guid
      - RegistrationPlate: string
      - Make: string
      - Model: string
      - Year: int(1950..current+1)
      - PayloadCapacityKg: decimal(>=0)
      - HasRamp: bool
  Tour:
    soft_delete: false
    unique: [VehicleId, Date, TourNumber]
    fields:
      - Id: Guid
      - VehicleId: Guid
      - Date: DateOnly
      - TourNumber: int(>0)
      - UnloadCount: int(>=0)
      - WeightKg: decimal(>=0)
      - DistanceKm: decimal(>0)
api_groups:
  vehicles:
    - POST /api/vehicles
    - GET /api/vehicles
    - GET /api/vehicles/{id}
    - PUT /api/vehicles/{id}
    - DELETE /api/vehicles/{id}
  tours:
    - POST /api/vehicles/{vehicleId}/tours
    - GET /api/vehicles/{vehicleId}/tours?date=
    - GET /api/tours/{id}
    - PUT /api/tours/{id}
    - DELETE /api/tours/{id}
  statistics:
    - GET /api/vehicles/{vehicleId}/daily-summary?date=
    - GET /api/vehicles/{vehicleId}/total-mileage
error_mapping:
  NotFoundException: 404
  ConflictException: 409
  DomainRuleViolationException: 400
  ValidationException: 400
  Default: 500
```

---

## 17) Zaključak

Ovaj projekat je jasno složen, konzistentan sa CQRS obrascem i dobar je kandidat za dalje proširenje (npr. autentikacija, audit log, pagination/filtering, testovi, migracije u repo-u, OpenAPI primeri). Dokument iznad je optimizovan tako da i čovek i LLM mogu brzo da lociraju odgovornost, pravila i mesta izmene bez dugog pretraživanja koda.
