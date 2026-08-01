# BackAlmacen - API REST de Gestion de Juguetes

API RESTful desarrollada en .NET (C#) aplicando Clean Architecture, CQRS, MediatR, Entity Framework Core In-Memory y Pruebas Unitarias. Necesario abrir en Visual Studio 2026.

---

## Tecnologias y Patrones Utilizados

* Lenguaje y Framework: .NET 8 / C#
* Arquitectura: Clean Architecture (Domain, Application, Persistence, API, Testing)
* Patrones: CQRS (Command Query Responsibility Segregation) con MediatR
* Persistencia: Entity Framework Core (In-Memory Database)
* Mapeo de Objetos: AutoMapper
* Versionamiento API: ASP.NET Core API Versioning (v1.0)
* Pruebas Unitarias: xUnit + Moq
* Documentacion: Swagger / OpenAPI

---

## Requisitos Previos

* .NET SDK 8.0 o superior.
* Visual Studio 2026, Visual Studio Code o JetBrains Rider.

---

## Configuracion y Ejecucion Local

La aplicacion utiliza una base de datos en memoria (In-Memory) que se inicializa y siembra con datos iniciales al arrancar la aplicacion. No requiere configuracion previa de servidores de bases de datos.

### 1. Clonar el repositorio
```bash
git clone [https://github.com/Anubis10/BackAlmancen.git](https://github.com/Anubis10/BackAlmancen.git)
cd BackAlmancen

### Restaurar dependencias
dotnet restore

### Ejecutar la API
dotnet run --project src/BackAlmacen/BackAlmacen.csproj