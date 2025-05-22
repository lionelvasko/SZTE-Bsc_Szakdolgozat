# SZTE TTIK Programtervező informatikus szak - Bsc Szakdolgozat

## Szakdolgozat Solution

### Leírás
A Szakdolgozat egy .NET 9 alapú megoldás, amely egy .NET MAUI frontend alkalmazást és több backend API-t tartalmaz. 
A projekt célja egy modern, platformfüggetlen alkalmazás létrehozása, amely integrálható külső szolgáltatásokkal, például már létező Somfy és Tuya API-val.

### Projektek

#### 1. Frontend:

- Technológia: .NET MAUI
- Funkciók:
  -   Felhasználói felület a Somfy és Tuya eszközök vezérléséhez.
  -	  Integráció a backend API-kkal.
  -	  Reszponzív dizájn több platformon (Windows, Android, iOS).
  -	  Sötét és világos téma támogatása
  -	  Globalizáció
  -	  Authentikáció és authorizáció

#### 2. AuthAPI:
- Technológia: ASP.NET Core
-	Funkciók:
  -	Felhasználói hitelesítés
  -	REST API végpontok a frontend számára + Swagger felület
    -	Főeszközök CRUD műveletei
    -	Mellékeszközök CRUD műveletei
    -	Felhasználói adatok CRUD műveletei
    -	Hitelesítés
   
#### 3. SomfyAPI:
- Technológia: ASP.NET Core Class Library
- Funkciók:
  - Authentikáció a Somfy felhős környezetével
  - Eszközök lekérése
  - Eszközök vezérlése (pl. redőnyök nyitása, zárása, megállítása).
  -	JSON-alapú kommunikáció.
 
#### 4. TuyaAPI
- Technológia: ASP.NET Core Class Library
- Funkciók:
  - Authentikáció a Tuya felhős környezetével
  - Eszközök lekérése
  - Eszközök vezérlése (pl. lámpák kapcsolása).
  -	JSON alapú kommunikáció.
