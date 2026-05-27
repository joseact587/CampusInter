# CampusInter

CampusInter es una aplicación académica para gestionar el registro de estudiantes, autenticación con JWT, consulta de materias, inscripción académica y visualización de compañeros por clase.

El proyecto está dividido en backend con Clean Architecture y frontend Angular standalone.

## Tabla de Contenido

- [Características](#características)
- [Arquitectura](#arquitectura)
- [Tecnologías](#tecnologías)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Requisitos](#requisitos)
- [Configuración](#configuración)
- [Ejecución](#ejecución)
- [Base de Datos y Seed](#base-de-datos-y-seed)
- [Credenciales de Prueba](#credenciales-de-prueba)
- [Endpoints Principales](#endpoints-principales)
- [Frontend](#frontend)
- [Notas de Desarrollo](#notas-de-desarrollo)

## Características

- Registro e inicio de sesión de estudiantes.
- Autenticación con JWT.
- Claims de usuario y estudiante separados.
- Autorización por roles y policies.
- Separación entre `Usuario` y `Estudiante`.
- Consulta de materias disponibles.
- Creación de inscripción con reglas académicas.
- Consulta de inscripción activa.
- Consulta de compañeros por materia.
- Consulta de estudiantes registrados.
- Consulta y actualización de perfil propio.
- Inhabilitación del perfil académico del estudiante.
- Frontend Angular con layout privado, interceptores, loading global y manejo global de errores.

## Arquitectura

El backend sigue Clean Architecture:

```text
CampusInter.Domain
  Entidades, enums, reglas de dominio y contratos base.

CampusInter.Application
  Casos de uso, DTOs, interfaces, servicios y mappings.

CampusInter.Infrastructure
  Entity Framework Core, repositorios, seguridad, persistencia y seed.

CampusInter.Api
  Controllers, autenticación JWT, CORS, Swagger y middleware global de errores.
```

La separación principal del dominio es:

```text
Usuario
  Identidad, correo, password hash, rol y estado de acceso.

Estudiante
  Perfil académico, documento, nombres, estado académico e inscripción.
```

Relación:

```text
Usuario 1 ---- 0..1 Estudiante
```

## Tecnologías

### Backend

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- JWT Bearer Authentication
- Swagger / OpenAPI
- AutoMapper

### Frontend

- Angular 19
- Standalone Components
- Angular Reactive Forms
- Angular Signals
- Angular HTTP Interceptors
- Font Awesome
- CSS global y CSS por componente

## Estructura del Proyecto

```text
CampusInter
├── 1.Front-End
│   └── campusinter.client
│       └── src
│           ├── app
│           │   ├── core
│           │   ├── features
│           │   └── shared
│           └── styles.css
│
├── 2.Back-End
│   ├── CampusInter.Api
│   ├── CampusInter.Application
│   ├── CampusInter.Domain
│   └── CampusInter.Infrastructure
│
└── CampusInter.slnx
```

## Requisitos

- .NET SDK 10
- Node.js
- npm
- SQL Server LocalDB
- Angular CLI, opcional si se usa `npm start`

## Configuración

### Backend

La cadena de conexión por defecto está en:

```text
2.Back-End/CampusInter.Api/appsettings.json
2.Back-End/CampusInter.Api/appsettings.Development.json
```

Valor actual:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CampusInterDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

La configuración JWT está en `appsettings.json`:

```json
"Jwt": {
  "Issuer": "CampusInter",
  "Audience": "CampusInter.Client",
  "SecretKey": "CAMBIAR_ESTA_CLAVE_EN_DESARROLLO_POR_UNA_MAS_SEGURA_DE_MINIMO_32_CARACTERES",
  "ExpirationMinutes": 60
}
```

Para producción se debe cambiar `SecretKey` por una clave segura y mover secretos fuera del repositorio.

### Frontend

La URL base de la API está en:

```text
1.Front-End/campusinter.client/src/environments/environment.ts
```

Valor actual:

```ts
export const environment = {
  production: false,
  apiBaseUrl: 'https://localhost:7173'
};
```

## Ejecución

### Ejecutar Backend

Desde la raíz del repositorio:

```bash
dotnet run --project ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj --launch-profile https
```

La API queda disponible en:

```text
https://localhost:7173
http://localhost:5127
```

Swagger:

```text
https://localhost:7173/swagger
```

### Ejecutar Frontend

Desde el proyecto Angular:

```bash
cd ./1.Front-End/campusinter.client
npm install
npm start
```

El frontend usa SSL local y normalmente queda disponible en:

```text
https://localhost:54163
```

## Base de Datos y Seed

En ambiente `Development`, el backend ejecuta automáticamente:

```csharp
await db.Database.MigrateAsync();
await ApplicationDbContextSeeder.SeedAsync(db, passwordHasher);
```

Esto significa que al iniciar la API:

- Se aplican migraciones pendientes.
- Se crean profesores de prueba.
- Se crean materias de prueba.
- Se crean usuarios estudiantes.
- Se crean estudiantes asociados a usuarios.
- Se crean inscripciones activas.

El seed incluye:

- 5 profesores.
- 10 materias.
- 15 usuarios con rol `Estudiante`.
- 15 estudiantes.
- 15 inscripciones activas.

## Credenciales de Prueba

Todos los usuarios seed tienen la misma contraseña:

```text
Prueba123
```

Ejemplos:

```text
juan.perez@test.com
ana.rodriguez@test.com
luis.garcia@test.com
```

## Endpoints Principales

### Autenticación

```http
POST /api/auth/register
POST /api/auth/login
```

### Materias

```http
GET /api/materias
```

Requiere token con policy `Estudiante`.

### Inscripciones

```http
POST /api/inscripciones
GET /api/inscripciones/mi-inscripcion
GET /api/inscripciones/mi-inscripcion/companeros
```

Requiere token con policy `Estudiante`.

### Estudiantes

```http
GET /api/estudiantes
GET /api/estudiantes/me
PUT /api/estudiantes/me
PATCH /api/estudiantes/me/inhabilitar
```

Requiere token con policy `Estudiante`.

## Frontend

El frontend está organizado por módulos funcionales:

```text
src/app
├── core
│   ├── auth
│   ├── config
│   ├── errors
│   ├── http
│   ├── layout
│   └── loading
│
├── features
│   ├── auth
│   ├── dashboard
│   ├── estudiantes
│   ├── inscripciones
│   └── materias
│
└── shared
```

### Core

- `AuthService`: login, registro y cierre de sesión.
- `CurrentUserStore`: estado del usuario autenticado con signals.
- `TokenStorageService`: persistencia en `localStorage`.
- `authInterceptor`: agrega `Authorization: Bearer`.
- `errorInterceptor`: interpreta errores HTTP y ProblemDetails.
- `loadingInterceptor`: controla loading global.
- `PrivateLayoutComponent`: layout privado con sidebar.

### Pantallas

- `/login`
- `/register`
- `/home`
- `/materias`
- `/mi-inscripcion`
- `/estudiantes`
- `/mi-perfil`

## Reglas de Negocio Relevantes

- Una inscripción debe tener exactamente 3 materias.
- Las materias seleccionadas no deben repetir profesor.
- El estudiante autenticado se identifica por el claim `estudianteId`.
- El JWT usa `sub` como `UsuarioId`.
- El rol actual usado por la aplicación es `Estudiante`.
- Al inhabilitar el perfil, el estudiante queda inactivo y se cierra la sesión local.

## Comandos Útiles

### Compilar Backend

```bash
dotnet build ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj
```

Si la API está corriendo y bloquea DLLs, se puede compilar con salida temporal:

```bash
dotnet build ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj -p:OutputPath=../../artifacts/api-build/
```

### Compilar Frontend

```bash
cd ./1.Front-End/campusinter.client
npm run build
```

## Notas de Desarrollo

- No se exponen `PasswordHash` ni datos sensibles en respuestas.
- El listado general de estudiantes puede mostrar información resumida.
- El perfil propio permite consultar y actualizar correo y documento.
- El manejo visual global de errores está centralizado en `ErrorService`.
- Los estilos globales reutilizables viven en:

```text
1.Front-End/campusinter.client/src/styles.css
```

Ahí se centralizan variables, botones principales, inputs, selects, cards base y encabezados comunes.

## Estado Actual

El proyecto ya cuenta con el flujo principal funcional:

1. Registro o login.
2. Consulta de panel principal.
3. Consulta de materias.
4. Creación de inscripción.
5. Consulta de inscripción activa.
6. Consulta de compañeros por clase.
7. Consulta de estudiantes.
8. Consulta, edición e inhabilitación del perfil.

