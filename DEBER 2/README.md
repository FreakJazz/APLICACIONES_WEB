# Sistema CRUD de Clientes en C#

Aplicación de consola para gestionar clientes utilizando programación orientada a objetos y SQLite como base de datos.

## Características

- Crear nuevos clientes
- Listar todos los clientes registrados
- Buscar cliente por ID
- Actualizar información de clientes
- Eliminar clientes
- Buscar clientes por nombre o apellido
- Base de datos SQLite local
- Validación de emails
- Interfaz de consola amigable

## Tecnologías

- C# (.NET 6.0)
- SQLite
- Programación Orientada a Objetos

## Estructura del Proyecto

```
DEBER 2/
├── Cliente.cs              - Clase modelo del cliente
├── DatabaseManager.cs      - Gestión de la conexión a la base de datos
├── ClienteRepositorio.cs   - Operaciones CRUD
├── Program.cs              - Menú principal y lógica de la aplicación
├── ClienteCRUD.csproj      - Archivo de configuración del proyecto
└── clientes.db             - Base de datos SQLite (se crea automáticamente)
```

## Cómo usar

### Requisitos previos
- .NET 6.0 SDK o superior instalado

### Compilar y ejecutar

```bash
cd "DEBER 2"
dotnet restore
dotnet build
dotnet run
```

### Opciones del menú

1. **Registrar nuevo cliente** - Ingresa los datos de un nuevo cliente
2. **Ver todos los clientes** - Muestra la lista completa de clientes
3. **Buscar cliente por ID** - Encuentra un cliente específico por su ID
4. **Actualizar información** - Modifica los datos de un cliente existente
5. **Eliminar cliente** - Borra un cliente de la base de datos
6. **Buscar por nombre** - Encuentra clientes por nombre o apellido
0. **Salir** - Cierra la aplicación

## Ejemplo de uso

Al ejecutar la aplicación, verás un menú interactivo donde puedes gestionar tus clientes de forma sencilla. La base de datos se crea automáticamente en la misma carpeta del proyecto.

## Autor

Jazmin Rodriguez - Mayo 2026
