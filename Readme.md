# 🌌 StarWarsMoviesApp

**StarWarsMoviesApp** es una API REST desarrollada con .NET 8 siguiendo los principios de **Clean Architecture**. Su propósito es brindar un sistema de autenticación de usuarios (registro e inicio de sesión) junto con un CRUD y un CRON para agregar películas del universo de Star Wars.

Esta API está diseñada para ser fácilmente testeable, escalable y mantenible, utilizando buenas prácticas modernas y herramientas robustas del ecosistema .NET.

---

## 🚀 Tecnologías utilizadas

- **.NET 8**
- **Entity Framework Core** (con base de datos MySQL)
- **FluentValidation** (para validar inputs de forma expresiva y limpia)
- **Swagger / OpenAPI** (para documentación interactiva)
- **JWT (Bearer Tokens)** (para autenticación)
- **Hangfire** (para ejecutar tareas en segundo plano y con dashboard)
- **xUnit**, **Moq**, **FluentAssertions** (para tests automatizados)
- **Docker + Docker Compose** (para despliegue y ejecución en entornos consistentes)

---

## 🧱 Arquitectura

El proyecto implementa **Clean Architecture**, separando responsabilidades en capas:

- **API**: capa de presentación (controladores)
- **Application**: capa de lógica de negocio, DTOs y servicios
- **Infrastructure**: interacción con base de datos y proveedores externos
- **Domain**: entidades centrales y lógica de dominio
- **Shared**: utilidades comunes (por ejemplo, manejo de resultados)

---

## 📄 Endpoints

Los endpoints disponibles incluyen:

- **Autenticación**: registro e inicio de sesión
- **Películas**: CRUD completo

Todos los endpoints están documentados de forma clara y accesible a través de **Swagger** en: http://localhost:8080/swagger


Para acceder a rutas protegidas, podés autenticarte en `/api/auth/login`, copiar el JWT y presionar "Authorize" en el botón de Swagger.

---

## 🔐 Seguridad (JWT Bearer Token)

Una vez logueado, el token JWT debe usarse con el prefijo `Bearer`, por ejemplo: Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR...


⚠️ Swagger agrega automáticamente el prefijo "Bearer", por lo que solo pegá el token, sin escribir "Bearer" al principio.

---

## 🛠️ Validaciones

Se usa **FluentValidation** para validar los modelos de entrada. Por ejemplo, las contraseñas deben contener:

- Al menos una letra mayúscula
- Al menos una letra minúscula
- Al menos un número
- Puede contener símbolos como `_`, `,`, `-`, `.` etc.

---

## 🧪 Tests

Se incluyen **tests unitarios y de integración** usando:

- `xUnit` para el framework de testing
- `Moq` para mocks
- `FluentAssertions` para aserciones legibles

Los tests cubren servicios de autenticación y lógica de negocio del dominio.

---

## 🐳 Cómo correr localmente con Docker

### Requisitos

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### Instrucciones

1. Cloná el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/StarWarsMoviesApp.git
   cd StarWarsMoviesApp

2. Levantá los contenedores:
   ```bash
    docker-compose up --build

3. Accedé a:
    - API: http://localhost:8080
    - Swagger: http://localhost:8080/swagger
    - Hangfire Dashboard: http://localhost:8080/jobs