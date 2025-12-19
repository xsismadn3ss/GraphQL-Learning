# GraphQL-Learning

Este proyecto es una implementación de una API utilizando **GraphQL** con **.NET 10** y **HotChocolate**, enfocada en la gestión de autores y libros.

## 🚀 Arquitectura del Proyecto

El proyecto sigue una arquitectura de capas orientada a servicios, diseñada para separar las responsabilidades de la lógica de negocio, el acceso a datos y la interfaz de GraphQL.

### Componentes Principales:
1.  **Capa de Presentación (GraphQL):** Utiliza **HotChocolate** para exponer `Queries` y `Mutations`. Se emplea el enfoque de "Type Extensions" (`[ExtendObjectType]`) para mantener el esquema modular.
2.  **Capa de Servicios (Business Logic):** Centraliza la lógica de negocio en clases como `AuthorService` y `BookService`. Esta capa interactúa directamente con el contexto de la base de datos.
3.  **Capa de Datos:** Implementada con **Entity Framework Core (SQL Server)**. Incluye el `AppDbContext` y las entidades del modelo con soporte para migraciones.
4.  **Middleware Personalizado:** Incluye un `ValidationMiddleware` que intercepta las peticiones GraphQL para ejecutar validaciones automáticas basadas en `Data Annotations`.

## 📁 Estructura de Carpetas

* **`/Exceptions`**: Excepciones personalizadas para manejo de errores como `NotFoundException` y `DuplicateEntityException`.
* **`/Middleware`**: Contiene el `ValidationMiddleware.cs` para la lógica de validación de entradas.
* **`/Migrations`**: Archivos de control de versiones de la base de datos generados por EF Core.
* **`/Models`**:
    * **Entidades:** `Author.cs` y `Book.cs` que definen los datos.
    * **`/Input`**: Modelos de entrada (records) para las mutaciones.
    * **`AppDbContext.cs`**: Configuración de EF Core y `DbSets`.
* **`/Mutations`**: Operaciones de escritura (Add, Update, Delete).
* **`/Queries`**: Operaciones de lectura con soporte para paginación, filtrado y ordenamiento.
* **`/Service`**: Lógica principal. Incluye `TypesMapper.cs` para el registro automático de tipos mediante reflexión.

## ⚙️ Características Especiales

* **Registro Dinámico:** Escaneo automático de tipos que terminan en `Service`, `Mutation` o `Query` para su registro en el contenedor de dependencias.
* **Validación de Modelos:** Integración de `Data Annotations` directamente en los inputs de GraphQL.
* **Paginación y Proyección:** Uso de directivas de HotChocolate para consultas eficientes.
