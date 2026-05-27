-- =============================================================================
-- BASE DE DATOS: Data Warehouse de Ventas (Modelo Estrella)
-- SGBD Compatible: PostgreSQL / MySQL / SQLite
-- Descripción: Script que crea un esquema tipo Data Warehouse orientado a la
--              toma de decisiones, incluyendo tablas de dimensiones y hechos,
--              datos de ejemplo y consultas avanzadas (JOINs, agregados,
--              subconsultas, vistas, funciones de ventana).
-- Autor: FreakJazz
-- =============================================================================

-- -----------------------------------------------------------------------------
-- PARTE 1: CREACIÓN DE TABLAS (ESQUEMA ESTRELLA)
-- -----------------------------------------------------------------------------

-- Tabla de dimensión: Tiempo
CREATE TABLE IF NOT EXISTS DIM_TIEMPO (
    id_tiempo    INTEGER PRIMARY KEY,
    fecha        DATE        NOT NULL,
    anio         INTEGER     NOT NULL,
    trimestre    INTEGER     NOT NULL,  -- 1..4
    mes          INTEGER     NOT NULL,  -- 1..12
    nombre_mes   VARCHAR(20) NOT NULL,
    semana       INTEGER     NOT NULL,
    dia_semana   VARCHAR(15) NOT NULL
);

-- Tabla de dimensión: Producto
CREATE TABLE IF NOT EXISTS DIM_PRODUCTO (
    id_producto  INTEGER PRIMARY KEY,
    nombre       VARCHAR(100) NOT NULL,
    categoria    VARCHAR(50)  NOT NULL,
    subcategoria VARCHAR(50),
    proveedor    VARCHAR(100),
    precio_lista DECIMAL(10,2) NOT NULL
);

-- Tabla de dimensión: Cliente
CREATE TABLE IF NOT EXISTS DIM_CLIENTE (
    id_cliente   INTEGER PRIMARY KEY,
    nombre       VARCHAR(100) NOT NULL,
    segmento     VARCHAR(50)  NOT NULL,  -- 'Corporativo', 'PYME', 'Particular'
    ciudad       VARCHAR(80),
    pais         VARCHAR(60)  NOT NULL
);

-- Tabla de dimensión: Región
CREATE TABLE IF NOT EXISTS DIM_REGION (
    id_region    INTEGER PRIMARY KEY,
    ciudad       VARCHAR(80)  NOT NULL,
    provincia    VARCHAR(80),
    pais         VARCHAR(60)  NOT NULL,
    zona         VARCHAR(40)  NOT NULL   -- 'Norte', 'Sur', 'Costa', 'Sierra'
);

-- Tabla de hechos: Ventas
CREATE TABLE IF NOT EXISTS FACT_VENTAS (
    id_venta        INTEGER PRIMARY KEY,
    id_tiempo       INTEGER      NOT NULL,
    id_producto     INTEGER      NOT NULL,
    id_cliente      INTEGER      NOT NULL,
    id_region       INTEGER      NOT NULL,
    cantidad        INTEGER      NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    descuento       DECIMAL(5,2)  DEFAULT 0.00,
    total_venta     DECIMAL(12,2) NOT NULL,
    FOREIGN KEY (id_tiempo)   REFERENCES DIM_TIEMPO(id_tiempo),
    FOREIGN KEY (id_producto) REFERENCES DIM_PRODUCTO(id_producto),
    FOREIGN KEY (id_cliente)  REFERENCES DIM_CLIENTE(id_cliente),
    FOREIGN KEY (id_region)   REFERENCES DIM_REGION(id_region)
);

-- -----------------------------------------------------------------------------
-- PARTE 2: DATOS DE EJEMPLO (INSERTS)
-- -----------------------------------------------------------------------------

-- DIM_TIEMPO
INSERT INTO DIM_TIEMPO VALUES (1, '2024-01-15', 2024, 1, 1, 'Enero',    3, 'Lunes');
INSERT INTO DIM_TIEMPO VALUES (2, '2024-02-10', 2024, 1, 2, 'Febrero',  6, 'Sábado');
INSERT INTO DIM_TIEMPO VALUES (3, '2024-03-22', 2024, 1, 3, 'Marzo',   12, 'Viernes');
INSERT INTO DIM_TIEMPO VALUES (4, '2024-04-05', 2024, 2, 4, 'Abril',   14, 'Viernes');
INSERT INTO DIM_TIEMPO VALUES (5, '2024-05-18', 2024, 2, 5, 'Mayo',    20, 'Sábado');
INSERT INTO DIM_TIEMPO VALUES (6, '2024-06-30', 2024, 2, 6, 'Junio',   26, 'Domingo');
INSERT INTO DIM_TIEMPO VALUES (7, '2024-07-14', 2024, 3, 7, 'Julio',   28, 'Domingo');
INSERT INTO DIM_TIEMPO VALUES (8, '2024-08-25', 2024, 3, 8, 'Agosto',  34, 'Domingo');
INSERT INTO DIM_TIEMPO VALUES (9, '2024-09-09', 2024, 3, 9, 'Septiembre', 37, 'Lunes');
INSERT INTO DIM_TIEMPO VALUES(10, '2024-10-20', 2024, 4,10, 'Octubre', 42, 'Domingo');
INSERT INTO DIM_TIEMPO VALUES(11, '2024-11-11', 2024, 4,11, 'Noviembre',46,'Lunes');
INSERT INTO DIM_TIEMPO VALUES(12, '2024-12-25', 2024, 4,12, 'Diciembre',52,'Miércoles');

-- DIM_PRODUCTO
INSERT INTO DIM_PRODUCTO VALUES (1, 'Laptop ProBook 450',  'Electrónica',  'Computadoras',  'HP Ecuador',       1200.00);
INSERT INTO DIM_PRODUCTO VALUES (2, 'Monitor UltraWide',   'Electrónica',  'Monitores',     'LG Ecuador',        450.00);
INSERT INTO DIM_PRODUCTO VALUES (3, 'Teclado Mecánico',    'Periféricos',  'Teclados',      'Logitech',          120.00);
INSERT INTO DIM_PRODUCTO VALUES (4, 'Mouse Inalámbrico',   'Periféricos',  'Ratones',       'Logitech',           45.00);
INSERT INTO DIM_PRODUCTO VALUES (5, 'Silla Ergonómica',    'Mobiliario',   'Sillas',        'Muebles Quito',     350.00);
INSERT INTO DIM_PRODUCTO VALUES (6, 'Escritorio Ejecutivo','Mobiliario',   'Escritorios',   'Muebles Quito',     600.00);
INSERT INTO DIM_PRODUCTO VALUES (7, 'Impresora Láser',     'Electrónica',  'Impresoras',    'Canon Ecuador',     280.00);
INSERT INTO DIM_PRODUCTO VALUES (8, 'Tablet Android 10"', 'Electrónica',  'Tablets',       'Samsung Ecuador',   400.00);
INSERT INTO DIM_PRODUCTO VALUES (9, 'Audífonos Bluetooth', 'Periféricos',  'Audio',         'JBL Ecuador',       150.00);
INSERT INTO DIM_PRODUCTO VALUES(10, 'Webcam HD 1080p',     'Periféricos',  'Cámaras',       'Logitech',           90.00);

-- DIM_CLIENTE
INSERT INTO DIM_CLIENTE VALUES (1, 'TechSolutions Cía. Ltda.',  'Corporativo', 'Quito',      'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (2, 'Universidad Central',       'Corporativo', 'Quito',      'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (3, 'Ferretería El Constructor', 'PYME',        'Guayaquil',  'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (4, 'Carlos Mendoza',            'Particular',  'Cuenca',     'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (5, 'Distribuidora Digital S.A.','Corporativo', 'Guayaquil',  'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (6, 'Colegio San José',          'PYME',        'Ambato',     'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (7, 'María Fernández',           'Particular',  'Loja',       'Ecuador');
INSERT INTO DIM_CLIENTE VALUES (8, 'ImportSur Cía. Ltda.',      'Corporativo', 'Cuenca',     'Ecuador');

-- DIM_REGION
INSERT INTO DIM_REGION VALUES (1, 'Quito',     'Pichincha',  'Ecuador', 'Sierra');
INSERT INTO DIM_REGION VALUES (2, 'Guayaquil', 'Guayas',     'Ecuador', 'Costa');
INSERT INTO DIM_REGION VALUES (3, 'Cuenca',    'Azuay',      'Ecuador', 'Sierra');
INSERT INTO DIM_REGION VALUES (4, 'Ambato',    'Tungurahua', 'Ecuador', 'Sierra');
INSERT INTO DIM_REGION VALUES (5, 'Loja',      'Loja',       'Ecuador', 'Sur');

-- FACT_VENTAS (id_venta, id_tiempo, id_producto, id_cliente, id_region, cantidad, precio_unit, descuento, total_venta)
INSERT INTO FACT_VENTAS VALUES ( 1,  1,  1, 1, 1,  5, 1200.00,  5.00,  5700.00);
INSERT INTO FACT_VENTAS VALUES ( 2,  1,  3, 1, 1, 10,  120.00,  0.00,  1200.00);
INSERT INTO FACT_VENTAS VALUES ( 3,  2,  2, 2, 1,  8,  450.00,  3.00,  3492.00);
INSERT INTO FACT_VENTAS VALUES ( 4,  2,  7, 2, 1,  4,  280.00,  0.00,  1120.00);
INSERT INTO FACT_VENTAS VALUES ( 5,  3,  5, 3, 2,  6,  350.00,  2.00,  2058.00);
INSERT INTO FACT_VENTAS VALUES ( 6,  3,  6, 3, 2,  3,  600.00,  0.00,  1800.00);
INSERT INTO FACT_VENTAS VALUES ( 7,  4,  4, 4, 3,  2,   45.00,  0.00,    90.00);
INSERT INTO FACT_VENTAS VALUES ( 8,  4,  9, 4, 3,  1,  150.00,  0.00,   150.00);
INSERT INTO FACT_VENTAS VALUES ( 9,  5,  8, 5, 2, 15,  400.00, 10.00,  5400.00);
INSERT INTO FACT_VENTAS VALUES (10,  5,  1, 5, 2,  3, 1200.00,  5.00,  3420.00);
INSERT INTO FACT_VENTAS VALUES (11,  6,  3, 6, 4, 20,  120.00,  0.00,  2400.00);
INSERT INTO FACT_VENTAS VALUES (12,  6, 10, 6, 4, 10,   90.00,  0.00,   900.00);
INSERT INTO FACT_VENTAS VALUES (13,  7,  2, 7, 5,  2,  450.00,  0.00,   900.00);
INSERT INTO FACT_VENTAS VALUES (14,  7,  9, 7, 5,  2,  150.00,  0.00,   300.00);
INSERT INTO FACT_VENTAS VALUES (15,  8,  1, 8, 3, 10, 1200.00,  8.00, 11040.00);
INSERT INTO FACT_VENTAS VALUES (16,  8,  5, 8, 3, 10,  350.00,  5.00,  3325.00);
INSERT INTO FACT_VENTAS VALUES (17,  9,  7, 1, 1,  2,  280.00,  0.00,   560.00);
INSERT INTO FACT_VENTAS VALUES (18,  9,  8, 2, 1,  5,  400.00,  3.00,  1940.00);
INSERT INTO FACT_VENTAS VALUES (19, 10,  6, 3, 2,  2,  600.00,  0.00,  1200.00);
INSERT INTO FACT_VENTAS VALUES (20, 10,  4, 5, 2, 30,   45.00,  0.00,  1350.00);
INSERT INTO FACT_VENTAS VALUES (21, 11,  1, 6, 4,  4, 1200.00,  5.00,  4560.00);
INSERT INTO FACT_VENTAS VALUES (22, 11,  2, 7, 5,  3,  450.00,  0.00,  1350.00);
INSERT INTO FACT_VENTAS VALUES (23, 12,  8, 8, 3,  8,  400.00,  5.00,  3040.00);
INSERT INTO FACT_VENTAS VALUES (24, 12,  1, 1, 1,  2, 1200.00,  0.00,  2400.00);

-- -----------------------------------------------------------------------------
-- PARTE 3: VISTAS
-- -----------------------------------------------------------------------------

-- Vista resumen de ventas con toda la información descriptiva
CREATE VIEW IF NOT EXISTS V_VENTAS_DETALLE AS
SELECT
    fv.id_venta,
    dt.fecha,
    dt.anio,
    dt.nombre_mes,
    dt.trimestre,
    dp.nombre        AS producto,
    dp.categoria,
    dc.nombre        AS cliente,
    dc.segmento,
    dr.ciudad        AS ciudad_venta,
    dr.zona,
    fv.cantidad,
    fv.precio_unitario,
    fv.descuento,
    fv.total_venta
FROM FACT_VENTAS     fv
JOIN DIM_TIEMPO   dt ON fv.id_tiempo   = dt.id_tiempo
JOIN DIM_PRODUCTO dp ON fv.id_producto = dp.id_producto
JOIN DIM_CLIENTE  dc ON fv.id_cliente  = dc.id_cliente
JOIN DIM_REGION   dr ON fv.id_region   = dr.id_region;

-- Vista de ventas mensuales por categoría de producto
CREATE VIEW IF NOT EXISTS V_VENTAS_MES_CATEGORIA AS
SELECT
    dt.anio,
    dt.mes,
    dt.nombre_mes,
    dp.categoria,
    COUNT(fv.id_venta)     AS num_transacciones,
    SUM(fv.cantidad)       AS unidades_vendidas,
    ROUND(SUM(fv.total_venta), 2) AS ingresos_totales
FROM FACT_VENTAS     fv
JOIN DIM_TIEMPO   dt ON fv.id_tiempo   = dt.id_tiempo
JOIN DIM_PRODUCTO dp ON fv.id_producto = dp.id_producto
GROUP BY dt.anio, dt.mes, dt.nombre_mes, dp.categoria;

-- -----------------------------------------------------------------------------
-- PARTE 4: CONSULTAS AVANZADAS PARA ANÁLISIS
-- -----------------------------------------------------------------------------

-- ======================================================
-- Q1: Ventas totales por categoría de producto (JOIN + GROUP BY + agregados)
-- ======================================================
SELECT
    dp.categoria,
    COUNT(fv.id_venta)              AS total_transacciones,
    SUM(fv.cantidad)                AS unidades_vendidas,
    ROUND(AVG(fv.total_venta), 2)   AS promedio_venta,
    ROUND(SUM(fv.total_venta), 2)   AS ingresos_totales
FROM FACT_VENTAS     fv
JOIN DIM_PRODUCTO dp ON fv.id_producto = dp.id_producto
GROUP BY dp.categoria
ORDER BY ingresos_totales DESC;

-- ======================================================
-- Q2: Top 5 clientes por ingreso generado (JOIN + GROUP BY + LIMIT)
-- ======================================================
SELECT
    dc.nombre                         AS cliente,
    dc.segmento,
    dc.ciudad,
    COUNT(fv.id_venta)                AS compras_realizadas,
    ROUND(SUM(fv.total_venta), 2)     AS total_gastado
FROM FACT_VENTAS    fv
JOIN DIM_CLIENTE dc ON fv.id_cliente = dc.id_cliente
GROUP BY dc.id_cliente, dc.nombre, dc.segmento, dc.ciudad
ORDER BY total_gastado DESC
LIMIT 5;

-- ======================================================
-- Q3: Ventas por trimestre y zona geográfica (JOIN múltiple + GROUP BY)
-- ======================================================
SELECT
    dt.anio,
    dt.trimestre,
    dr.zona,
    ROUND(SUM(fv.total_venta), 2)  AS ingresos,
    SUM(fv.cantidad)               AS unidades
FROM FACT_VENTAS    fv
JOIN DIM_TIEMPO  dt ON fv.id_tiempo  = dt.id_tiempo
JOIN DIM_REGION  dr ON fv.id_region  = dr.id_region
GROUP BY dt.anio, dt.trimestre, dr.zona
ORDER BY dt.anio, dt.trimestre, ingresos DESC;

-- ======================================================
-- Q4: Productos con ingresos superiores al promedio general (subconsulta)
-- ======================================================
SELECT
    dp.nombre        AS producto,
    dp.categoria,
    ROUND(SUM(fv.total_venta), 2) AS ingresos_producto
FROM FACT_VENTAS     fv
JOIN DIM_PRODUCTO dp ON fv.id_producto = dp.id_producto
GROUP BY dp.id_producto, dp.nombre, dp.categoria
HAVING SUM(fv.total_venta) > (
    SELECT AVG(ingresos_por_producto)
    FROM (
        SELECT SUM(total_venta) AS ingresos_por_producto
        FROM FACT_VENTAS
        GROUP BY id_producto
    ) AS sub
)
ORDER BY ingresos_producto DESC;

-- ======================================================
-- Q5: Comparativa mensual de ventas año 2024 (JOIN + GROUP BY + ORDER)
-- ======================================================
SELECT
    dt.mes,
    dt.nombre_mes,
    COUNT(fv.id_venta)             AS num_ventas,
    SUM(fv.cantidad)               AS unidades,
    ROUND(SUM(fv.total_venta), 2)  AS ingresos
FROM FACT_VENTAS    fv
JOIN DIM_TIEMPO  dt ON fv.id_tiempo = dt.id_tiempo
WHERE dt.anio = 2024
GROUP BY dt.mes, dt.nombre_mes
ORDER BY dt.mes;

-- ======================================================
-- Q6: Segmento de clientes con mayor descuento promedio (JOIN + GROUP BY + HAVING)
-- ======================================================
SELECT
    dc.segmento,
    ROUND(AVG(fv.descuento), 2)   AS descuento_promedio,
    ROUND(SUM(fv.total_venta), 2) AS ingresos_totales
FROM FACT_VENTAS    fv
JOIN DIM_CLIENTE dc ON fv.id_cliente = dc.id_cliente
GROUP BY dc.segmento
HAVING AVG(fv.descuento) > 0
ORDER BY descuento_promedio DESC;

-- ======================================================
-- Q7: Ventas acumuladas por mes (JOIN + SUM acumulada con subconsulta correlacionada)
-- ======================================================
SELECT
    t1.mes,
    t1.nombre_mes,
    ROUND(t1.ingresos_mes, 2) AS ingresos_mes,
    ROUND(
        (SELECT SUM(fv2.total_venta)
         FROM FACT_VENTAS fv2
         JOIN DIM_TIEMPO dt2 ON fv2.id_tiempo = dt2.id_tiempo
         WHERE dt2.anio = 2024 AND dt2.mes <= t1.mes),
        2
    ) AS ingresos_acumulados
FROM (
    SELECT dt.mes, dt.nombre_mes, SUM(fv.total_venta) AS ingresos_mes
    FROM FACT_VENTAS fv
    JOIN DIM_TIEMPO dt ON fv.id_tiempo = dt.id_tiempo
    WHERE dt.anio = 2024
    GROUP BY dt.mes, dt.nombre_mes
) AS t1
ORDER BY t1.mes;

-- ======================================================
-- Q8: Productos más vendidos por zona geográfica (JOIN múltiple + GROUP BY + subconsulta)
-- ======================================================
SELECT
    dr.zona,
    dp.nombre     AS producto_mas_vendido,
    SUM(fv.cantidad) AS unidades_totales
FROM FACT_VENTAS     fv
JOIN DIM_REGION  dr ON fv.id_region  = dr.id_region
JOIN DIM_PRODUCTO dp ON fv.id_producto = dp.id_producto
GROUP BY dr.zona, dp.id_producto, dp.nombre
HAVING SUM(fv.cantidad) = (
    SELECT MAX(sub.unidades)
    FROM (
        SELECT fv2.id_region, fv2.id_producto, SUM(fv2.cantidad) AS unidades
        FROM FACT_VENTAS fv2
        GROUP BY fv2.id_region, fv2.id_producto
    ) AS sub
    WHERE sub.id_region = fv.id_region
)
ORDER BY dr.zona;

-- ======================================================
-- Q9: Análisis de participación de ingresos por categoría (JOIN + subconsulta + porcentaje)
-- ======================================================
SELECT
    dp.categoria,
    ROUND(SUM(fv.total_venta), 2)                                   AS ingresos_categoria,
    ROUND(
        SUM(fv.total_venta) * 100.0 / (SELECT SUM(total_venta) FROM FACT_VENTAS),
        2
    )                                                               AS porcentaje_participacion
FROM FACT_VENTAS     fv
JOIN DIM_PRODUCTO dp ON fv.id_producto = dp.id_producto
GROUP BY dp.categoria
ORDER BY ingresos_categoria DESC;

-- ======================================================
-- Q10: Consulta sobre la vista V_VENTAS_DETALLE (uso de vista)
-- ======================================================
SELECT
    zona,
    segmento,
    categoria,
    COUNT(id_venta)             AS transacciones,
    ROUND(SUM(total_venta), 2)  AS ingresos
FROM V_VENTAS_DETALLE
WHERE anio = 2024
GROUP BY zona, segmento, categoria
ORDER BY zona, ingresos DESC;
