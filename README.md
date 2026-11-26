# CercaCafe Web API — Guía de Configuración y Despliegue Local

Este proyecto es una Web API desarrollada en **.NET 8** bajo una arquitectura de 3 capas (**API, BLL, DAL**) para la gestión y localización de cafeterías.

---

## Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Microsoft SQL Server 2022](https://www.microsoft.com/sql-server/) o **SQL Server Express**
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (con la carga de trabajo *Desarrollo de ASP.NET y web*)
- [Postman](https://www.postman.com/) (opcional, para pruebas de endpoints HTTP)

---

## Pasos para Replicar la Configuración en Entorno Local

### 1. Clonar o Descomprimir el Proyecto
Descarga el repositorio y abre la solución `CercaCafe.sln` en **Visual Studio 2022**.

### 2. Restaurar la Base de Datos
1. Abre **SQL Server Management Studio (SSMS)** y conéctate a tu instancia local (por ejemplo, `localhost\SQLEXPRESS`).
2. Abre el archivo `CercaCafe_Backup_5.sql` incluido en el proyecto.
3. Ejecuta el script completo (**F5** o botón *Execute*). Esto creará automáticamente la base de datos `CercaCafe`, sus tablas y los datos iniciales.

### 3. Configurar la Cadena de Conexión
Abre el archivo `appsettings.json` ubicado en la carpeta del proyecto `CercaCafe.API` y verifica que la cadena apunte a tu instancia local y a la base de datos `CercaCafe`:

```json
{
  "ConnectionStrings": {
    "CercaCafeConnection": "Server=localhost\\SQLEXPRESS;Database=CercaCafe;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}