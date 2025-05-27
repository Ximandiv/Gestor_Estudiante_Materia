# Gestor Estudiante Materia

Un sistema de gestión de estudiantes y materias desarrollado con ASP.NET Core (.NET 8), Razor Pages y Entity Framework Core (SQLite).  
El proyecto demuestra Arquitectura Limpia (Clean Architecture), principios SOLID y el Patrón Repositorio para un código mantenible y testeable.

---

## Funcionalidades

- **Gestión de Estudiantes:** Crear, editar, eliminar y listar estudiantes.
- **Gestión de Materias:** Crear, editar, eliminar y listar materias.
- **Asignación de Materias:** Asignar y desasignar materias a estudiantes con reglas de negocio (por ejemplo, máximo de materias de muchos créditos).
- **Separación de Responsabilidades:** Capas de Aplicación, Dominio y Persistencia.
- **Pruebas Unitarias:** Proyecto de pruebas con xUnit y Moq para servicios/controladores.

---

## Estructura del Proyecto

- **Gestor_Estudiante_Materia/**: Aplicación principal ASP.NET Core Razor Pages (UI y Controladores)
- **Application/**: Lógica de aplicación, interfaces de servicios e implementaciones
- **Domain/**: Entidades, POCOs y ViewModels
- **Persistence/**: DbContext de EF Core e implementaciones de repositorios
- **Testing/**: Proyecto de pruebas unitarias con xUnit

---

## Instrucciones de Configuración

### 1. Clona el repositorio

git clone <tu-url-del-repo> cd <raíz-del-repo>

### 2. Configura la cadena de conexión

Este proyecto utiliza SQLite.  
**Configura la cadena de conexión en los secretos de usuario** (recomendado para desarrollo local):
dotnet user-secrets init --project Gestor_Estudiante_Materia dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=gestor.db" --project Gestor_Estudiante_Materia

Alternativamente, edita `appsettings.json` en `Gestor_Estudiante_Materia`:

"ConnectionStrings": { "DefaultConnection": "Data Source=gestor.db" }

### 3. Ejecuta las migraciones de la base de datos

Desde la **carpeta raíz** de la solución, ejecuta:

dotnet ef migrations add InitialCreate --project Persistence --startup-project Gestor_Estudiante_Materia 
dotnet ef database update --project Persistence --startup-project Gestor_Estudiante_Materia

Esto creará la base de datos SQLite y el esquema.

### 4. Ejecuta la aplicación

dotnet run --project Gestor_Estudiante_Materia

Visita [https://localhost:7244/](https://localhost:7244/) o [http://localhost:5290](http://localhost:5290).

---

## Arquitectura

### Arquitectura Limpia (Clean Architecture)

- **Capa de Dominio:** Contiene las entidades de negocio, POCOs y ViewModels. No depende de otras capas.
- **Capa de Aplicación:** Contiene interfaces e implementaciones de servicios, lógica de negocio y contratos.
- **Capa de Persistencia:** Contiene el DbContext de EF Core e implementaciones de repositorios.
- **Capa de Presentación:** Aplicación ASP.NET Core Razor Pages (controladores, vistas).

### Patrón Repositorio

- Todo el acceso a datos está abstraído mediante interfaces de repositorio (por ejemplo, `IStudentRepository`, `ICourseRepository`).
- Los controladores y servicios dependen de interfaces, no de implementaciones concretas de EF Core.

### Principios SOLID

- **Responsabilidad Única:** Cada clase tiene una sola responsabilidad (por ejemplo, los repositorios gestionan datos, los servicios la lógica de negocio).
- **Abierto/Cerrado:** Se pueden agregar nuevas funcionalidades mediante nuevas clases/interfaces sin modificar el código existente.
- **Sustitución de Liskov:** Se usan interfaces y clases base de manera adecuada.
- **Segregación de Interfaces:** Interfaces específicas para repositorios y servicios.
- **Inversión de Dependencias:** Los módulos de alto nivel dependen de abstracciones, no de implementaciones concretas.

---

## Pruebas

- **Las pruebas unitarias** están en el proyecto `Testing`.
- Se utiliza **xUnit** y **Moq** para simular servicios y repositorios.
- Ejecuta las pruebas con: dotnet test


---

## Notas

- Toda la inyección de dependencias se configura en `Program.cs`.
- El proyecto está listo para ser extendido (por ejemplo, autenticación, más reglas de negocio).
- Para comandos de EF Core CLI, usa siempre la carpeta raíz y especifica `--project Persistence --startup-project Gestor_Estudiante_Materia`.

---

## Licencia

Ninguna
