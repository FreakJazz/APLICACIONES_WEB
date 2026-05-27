# DEBER 3 – Bases de Datos para la Toma de Decisiones y Data Warehouse

## Descripción del entregable

Este deber comprende:

| Archivo | Descripción |
|---------|-------------|
| `RESUMEN.md` | Resumen escrito en español con revisión bibliográfica, definiciones de Data Warehouse, características comparativas y diagramas |
| `database_warehouse.sql` | Script SQL completo con esquema de Data Warehouse tipo estrella, datos de ejemplo y 10 consultas avanzadas |
| `README.md` | Este archivo con instrucciones de ejecución |

---

## Contenido del script SQL

El script `database_warehouse.sql` implementa un **mini Data Warehouse de Ventas** con arquitectura de esquema estrella:

### Tablas de dimensiones
- `DIM_TIEMPO` – dimensión temporal (fecha, año, trimestre, mes)
- `DIM_PRODUCTO` – catálogo de productos (nombre, categoría, proveedor, precio)
- `DIM_CLIENTE` – información de clientes (nombre, segmento, ciudad, país)
- `DIM_REGION` – ubicaciones geográficas (ciudad, provincia, zona)

### Tabla de hechos
- `FACT_VENTAS` – transacciones de venta con métricas cuantitativas (cantidad, precio, descuento, total)

### Vistas analíticas
- `V_VENTAS_DETALLE` – detalle completo de cada venta con todas sus dimensiones
- `V_VENTAS_MES_CATEGORIA` – resumen mensual de ventas por categoría de producto

### Consultas avanzadas incluidas

| # | Técnica SQL demostrada | Descripción |
|---|------------------------|-------------|
| Q1 | `JOIN` + `GROUP BY` + agregados (`COUNT`, `SUM`, `AVG`) | Ventas totales por categoría |
| Q2 | `JOIN` + `GROUP BY` + `LIMIT` | Top 5 clientes por ingreso |
| Q3 | `JOIN` múltiple + `GROUP BY` | Ventas por trimestre y zona geográfica |
| Q4 | Subconsulta anidada + `HAVING` | Productos con ingresos superiores al promedio |
| Q5 | `JOIN` + `GROUP BY` + filtro `WHERE` | Comparativa mensual 2024 |
| Q6 | `JOIN` + `GROUP BY` + `HAVING` con agregado | Segmentos con mayor descuento |
| Q7 | Subconsulta correlacionada | Ventas acumuladas por mes |
| Q8 | `JOIN` múltiple + subconsulta correlacionada + `HAVING` | Producto más vendido por zona |
| Q9 | Subconsulta escalar + porcentaje | Participación de ingresos por categoría |
| Q10 | Consulta sobre vista | Análisis cruzado zona/segmento/categoría |

---

## Cómo ejecutar el script

### Opción A – PostgreSQL

```bash
# Crear la base de datos (solo la primera vez)
createdb dw_ventas

# Ejecutar el script
psql -U <tu_usuario> -d dw_ventas -f database_warehouse.sql

# Conectarse para explorar
psql -U <tu_usuario> -d dw_ventas
```

### Opción B – MySQL / MariaDB

```sql
-- Desde la consola de MySQL:
CREATE DATABASE dw_ventas CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE dw_ventas;
SOURCE /ruta/completa/database_warehouse.sql;
```

O desde la terminal:

```bash
mysql -u <tu_usuario> -p dw_ventas < database_warehouse.sql
```

> **Nota:** Para MySQL, reemplaza `CREATE VIEW IF NOT EXISTS` por `CREATE OR REPLACE VIEW` si tu versión no soporta esa sintaxis.

### Opción C – SQLite (sin instalación adicional)

```bash
sqlite3 dw_ventas.db < database_warehouse.sql
sqlite3 dw_ventas.db
```

### Opción D – DB Browser for SQLite (interfaz gráfica)

1. Abre **DB Browser for SQLite** (descarga gratuita en https://sqlitebrowser.org/).
2. Clic en **New Database** → guarda como `dw_ventas.db`.
3. Ve a la pestaña **Execute SQL**.
4. Pega el contenido de `database_warehouse.sql` y presiona **Execute all** (F5).

---

## Verificar la carga de datos

Después de ejecutar el script, puedes verificar la carga con:

```sql
SELECT COUNT(*) FROM DIM_TIEMPO;     -- debe devolver 12
SELECT COUNT(*) FROM DIM_PRODUCTO;   -- debe devolver 10
SELECT COUNT(*) FROM DIM_CLIENTE;    -- debe devolver 8
SELECT COUNT(*) FROM DIM_REGION;     -- debe devolver 5
SELECT COUNT(*) FROM FACT_VENTAS;    -- debe devolver 24
```

---

## Nota sobre la participación en Canva

La actividad solicitaba participar en un enlace de Canva para agregar las características de las bases de datos y los Data Warehouses. Dicha participación debe realizarse manualmente siguiendo estos pasos:

1. Abrir el enlace de Canva proporcionado por el docente.
2. Agregar un elemento de texto o tarjeta con tu nombre y las características indicadas (disponibles en `RESUMEN.md`, secciones 4 y 5).
3. Guardar los cambios.
4. Tomar una captura de pantalla de tu participación:
   - **Windows:** `Win + Shift + S`
   - **Mac:** `Cmd + Shift + 4`
5. Subir la captura al repositorio o al aula virtual según se solicite.

---

## Referencias

Consultar `RESUMEN.md` para el listado completo de fuentes bibliográficas utilizadas.
