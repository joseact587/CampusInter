# CampusInter

CampusInter es una aplicación académica para registro de estudiantes, autenticación con JWT, consulta de materias, inscripción académica, consulta de compañeros por clase y gestión del perfil del estudiante.

El proyecto está dividido en backend con Clean Architecture y frontend Angular standalone.

## Menú

- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Migración y Base de Datos](#migración-y-base-de-datos)
- [Cómo Ejecutar el Proyecto](#cómo-ejecutar-el-proyecto)
- [Credenciales Iniciales](#credenciales-iniciales)
- [Datos del Seeder](#datos-del-seeder)
- [Arquitectura](#arquitectura)
- [Patrones y Decisiones Técnicas](#patrones-y-decisiones-técnicas)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Funcionalidades Principales](#funcionalidades-principales)
- [Endpoints Principales](#endpoints-principales)
- [Reglas de Negocio](#reglas-de-negocio)
- [Frontend](#frontend)
- [Seguridad](#seguridad)
- [Comandos Útiles](#comandos-útiles)
- [Estado del Proyecto](#estado-del-proyecto)

## Tecnologías Utilizadas

### Backend

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- JWT Bearer Authentication
- Swagger / OpenAPI
- AutoMapper
- DataAnnotations
- Clean Architecture

### Frontend

- Angular 19
- Standalone Components
- Angular Reactive Forms
- Angular Signals
- Angular HTTP Interceptors
- Font Awesome
- CSS global y CSS por componente

### Base de Datos

- SQL Server LocalDB
- Entity Framework Core Migrations
- Seeder automático en ambiente `Development`

## Migración y Base de Datos

La cadena de conexión está configurada en:

```text
2.Back-End/CampusInter.Api/appsettings.json
2.Back-End/CampusInter.Api/appsettings.Development.json
```

Valor por defecto:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CampusInterDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

### Aplicar Migraciones

Desde la raíz del repositorio:

```bash
dotnet ef database update --project ./2.Back-End/CampusInter.Infrastructure/CampusInter.Infrastructure.csproj --startup-project ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj
```

### Migración Automática en Development

En ambiente `Development`, la API ejecuta automáticamente las migraciones pendientes al iniciar:

```csharp
await db.Database.MigrateAsync();
await ApplicationDbContextSeeder.SeedAsync(db, passwordHasher);
```

Esto significa que al correr el backend en desarrollo:

- Se crea la base de datos si no existe.
- Se aplican migraciones pendientes.
- Se ejecuta el seeder si las tablas están vacías.

### Crear una Nueva Migración

Si se cambia el modelo de dominio o la configuración de Entity Framework:

```bash
dotnet ef migrations add NombreDeLaMigracion --project ./2.Back-End/CampusInter.Infrastructure/CampusInter.Infrastructure.csproj --startup-project ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj --output-dir Persistence/Migrations
```

Luego se aplica la migración:

```bash
dotnet ef database update --project ./2.Back-End/CampusInter.Infrastructure/CampusInter.Infrastructure.csproj --startup-project ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj
```

## Cómo Ejecutar el Proyecto

### 1. Ejecutar Backend

Desde la raíz del repositorio:

```bash
dotnet run --project ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj --launch-profile https
```

La API queda disponible normalmente en:

```text
https://localhost:7173
http://localhost:5127
```

Swagger:

```text
https://localhost:7173/swagger
```

### 2. Ejecutar Frontend

Desde el proyecto Angular:

```bash
cd ./1.Front-End/campusinter.client
npm install
npm start
```

El frontend queda disponible normalmente en:

```text
https://localhost:54163
```

### 3. Configurar URL de la API en Angular

El frontend consume la API desde:

```text
1.Front-End/campusinter.client/src/environments/environment.ts
```

Ejemplo:

```ts
export const environment = {
  production: false,
  apiBaseUrl: 'https://localhost:7173'
};
```

## Credenciales Iniciales

El seeder crea estudiantes con la contraseña:

```text
Prueba123
```

Ejemplos de usuarios:

```text
juan.perez@test.com
ana.rodriguez@test.com
luis.garcia@test.com
```

## Datos del Seeder

El seeder crea:

- 5 profesores.
- 10 materias.
- 10 usuarios con rol `Estudiante`.
- 10 estudiantes asociados a usuarios.
- 10 inscripciones activas.

Nota importante:

- La materia `Matematicas` se crea en el catálogo.
- Ninguna inscripción inicial selecciona `Matematicas`.
- Esto deja una materia disponible sin estudiantes inscritos.

El seeder es idempotente: si ya existen registros en las tablas principales, no duplica datos. Si se necesita cargar el seed desde cero, se debe limpiar o recrear la base de datos.

## Arquitectura

El backend sigue Clean Architecture:

```text
CampusInter.Domain
  Entidades, enums y reglas de dominio.

CampusInter.Application
  Casos de uso, DTOs, interfaces, servicios y mappings.

CampusInter.Infrastructure
  Entity Framework Core, repositorios, seguridad, persistencia y seed.

CampusInter.Api
  Controllers, JWT, CORS, Swagger y manejo global de errores.
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

## Patrones y Decisiones Técnicas

- Clean Architecture para separar responsabilidades.
- Repository Pattern para abstraer persistencia.
- Unit of Work para confirmar cambios.
- Service Layer para casos de uso.
- DTOs para contratos de entrada y salida.
- AutoMapper para mapeos entre dominio y respuestas.
- Dependency Injection en backend y frontend.
- Guards en Angular para proteger rutas privadas.
- Interceptors en Angular para JWT, errores y loading.
- Store con Signals para mantener el usuario actual.
- ProblemDetails para errores de API.

## Estructura del Proyecto

```text
CampusInter
|-- 1.Front-End
|   `-- campusinter.client
|       `-- src
|           |-- app
|           |   |-- core
|           |   |-- features
|           |   `-- shared
|           `-- styles.css
|
|-- 2.Back-End
|   |-- CampusInter.Api
|   |-- CampusInter.Application
|   |-- CampusInter.Domain
|   `-- CampusInter.Infrastructure
|
`-- CampusInter.slnx
```

## Funcionalidades Principales

- Registro de estudiantes.
- Login con JWT.
- Autorización por rol `Estudiante`.
- Consulta de materias.
- Inscripción de exactamente 3 materias.
- Restricción de profesores repetidos en la inscripción.
- Consulta de inscripción activa.
- Cancelación de inscripción activa.
- Consulta de compañeros por materia.
- Consulta de estudiantes.
- Consulta y actualización del perfil propio.
- Habilitación e inhabilitación del perfil académico.
- Menú privado según estado del estudiante.
- Alertas globales temporales para errores y acciones exitosas.
- Loading global.

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

### Inscripciones

```http
POST /api/inscripciones
GET /api/inscripciones/mi-inscripcion
GET /api/inscripciones/mi-inscripcion/companeros
DELETE /api/inscripciones/mi-inscripcion
```

### Estudiantes

```http
GET /api/estudiantes
GET /api/estudiantes/me
PUT /api/estudiantes/me
PATCH /api/estudiantes/me/inhabilitar
PATCH /api/estudiantes/me/habilitar
```

## Reglas de Negocio

- Una inscripción debe tener exactamente 3 materias.
- Las materias seleccionadas no pueden repetir profesor.
- Cada materia tiene 3 créditos.
- La inscripción debe sumar 9 créditos.
- Un estudiante solo puede tener una inscripción activa.
- Una inscripción cancelada no se elimina; queda con estado `Cancelada`.
- Un estudiante inactivo no puede crear inscripciones.
- Si el estudiante está inactivo, en el frontend solo puede acceder a `Mi Perfil`.
- El correo del perfil no se puede modificar desde la pantalla de perfil.

## Frontend

El frontend está organizado por módulos funcionales:

```text
src/app
|-- core
|   |-- auth
|   |-- config
|   |-- errors
|   |-- http
|   |-- layout
|   `-- loading
|
|-- features
|   |-- auth
|   |-- dashboard
|   |-- estudiantes
|   |-- inscripciones
|   `-- materias
|
`-- shared
```

### Core Frontend

- `AuthService`: login, registro y cierre de sesión.
- `CurrentUserStore`: estado del usuario autenticado con signals.
- `TokenStorageService`: persistencia en `localStorage`.
- `authInterceptor`: agrega `Authorization: Bearer`.
- `errorInterceptor`: interpreta errores HTTP y ProblemDetails.
- `loadingInterceptor`: controla loading global.
- `ErrorService`: alertas globales temporales.
- `PrivateLayoutComponent`: layout privado con sidebar.

### Pantallas

```text
/login
/register
/home
/materias
/mi-inscripcion
/estudiantes
/mi-perfil
```

## Seguridad

El JWT incluye:

- `sub`: identificador del `Usuario`.
- `estudianteId`: identificador del perfil académico.
- `email`: correo del usuario.
- `role`: rol del usuario.

Los endpoints privados usan policy de autorización para el rol `Estudiante`.

## Comandos Útiles

### Compilar Backend

```bash
dotnet build ./2.Back-End/CampusInter.Api/CampusInter.Api.csproj
```

### Compilar Frontend

```bash
cd ./1.Front-End/campusinter.client
npm run build
```

## Estado del Proyecto

El flujo principal implementado incluye:

1. Registro o login.
2. Consulta del panel principal.
3. Consulta de materias.
4. Creación de inscripción.
5. Consulta de inscripción activa.
6. Cancelación de inscripción.
7. Nueva inscripción después de cancelar.
8. Consulta de compañeros por clase.
9. Consulta de estudiantes.
10. Consulta y actualización del perfil.
11. Habilitación e inhabilitación del estudiante.
