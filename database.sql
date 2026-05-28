-- =========================================================
-- PROYECTO: SISTEMA DE VENTAS
-- OBJETIVO:
-- Crear una base de datos relacional que permita almacenar
-- información de clientes, productos y ventas para apoyar
-- la toma de decisiones mediante consultas avanzadas.
-- =========================================================


-- =========================================================
-- CREACIÓN DE LA BASE DE DATOS
-- =========================================================

-- Crear la base de datos

CREATE DATABASE sistema_ventas;

USE sistema_ventas;


-- =========================================
-- Tabla de clientes
-- =========================================

CREATE TABLE clientes (
    id_cliente INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    correo VARCHAR(150),
    ciudad VARCHAR(100)
);


-- =========================================
-- Tabla de categorías
-- =========================================

CREATE TABLE categorias (
    id_categoria INT PRIMARY KEY AUTO_INCREMENT,
    nombre_categoria VARCHAR(100) NOT NULL
);


-- =========================================
-- Tabla de productos
-- =========================================

CREATE TABLE productos (
    id_producto INT PRIMARY KEY AUTO_INCREMENT,
    nombre_producto VARCHAR(150) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL,
    id_categoria INT,

    FOREIGN KEY (id_categoria)
    REFERENCES categorias(id_categoria)
);


-- =========================================
-- Tabla de ventas
-- =========================================

CREATE TABLE ventas (
    id_venta INT PRIMARY KEY AUTO_INCREMENT,
    fecha_venta DATE NOT NULL,
    total DECIMAL(10,2),
    id_cliente INT,

    FOREIGN KEY (id_cliente)
    REFERENCES clientes(id_cliente)
);


-- =========================================
-- Tabla detalle de ventas
-- =========================================

CREATE TABLE detalle_ventas (
    id_detalle INT PRIMARY KEY AUTO_INCREMENT,
    id_venta INT,
    id_producto INT,
    cantidad INT NOT NULL,
    subtotal DECIMAL(10,2),

    FOREIGN KEY (id_venta)
    REFERENCES ventas(id_venta),

    FOREIGN KEY (id_producto)
    REFERENCES productos(id_producto)
);


-- =========================================
-- Insertar clientes
-- =========================================

INSERT INTO clientes (nombre, apellido, correo, ciudad)
VALUES
('Juan', 'Pérez', 'juan@gmail.com', 'Quito'),
('María', 'López', 'maria@gmail.com', 'Guayaquil'),
('Carlos', 'Mendoza', 'carlos@gmail.com', 'Cuenca'),
('Ana', 'Torres', 'ana@gmail.com', 'Ambato'),
('Luis', 'Vera', 'luis@gmail.com', 'Manta');


-- =========================================
-- Insertar categorías
-- =========================================

INSERT INTO categorias (nombre_categoria)
VALUES
('Laptops'),
('Celulares'),
('Accesorios'),
('Monitores');


-- =========================================
-- Insertar productos
-- =========================================

INSERT INTO productos
(nombre_producto, precio, stock, id_categoria)
VALUES
('Laptop Lenovo', 850.00, 10, 1),
('Laptop HP', 920.00, 8, 1),
('iPhone 15', 1200.00, 5, 2),
('Samsung S24', 980.00, 7, 2),
('Mouse Logitech', 25.00, 30, 3),
('Teclado Redragon', 45.00, 20, 3),
('Monitor LG 24', 210.00, 12, 4);


-- =========================================
-- Insertar ventas
-- =========================================

INSERT INTO ventas
(fecha_venta, total, id_cliente)
VALUES
('2026-05-01', 875.00, 1),
('2026-05-02', 1225.00, 2),
('2026-05-03', 1025.00, 3),
('2026-05-04', 280.00, 1),
('2026-05-05', 980.00, 4);


-- =========================================
-- Insertar detalle de ventas
-- =========================================

INSERT INTO detalle_ventas
(id_venta, id_producto, cantidad, subtotal)
VALUES

(1, 1, 1, 850.00),
(1, 5, 1, 25.00),

(2, 3, 1, 1200.00),
(2, 5, 1, 25.00),

(3, 4, 1, 980.00),
(3, 6, 1, 45.00),

(4, 7, 1, 210.00),
(4, 5, 1, 25.00),
(4, 6, 1, 45.00),

(5, 4, 1, 980.00);


-- =========================================
-- CONSULTAS
-- =========================================


-- Ver todos los clientes

SELECT * FROM clientes;


-- Ver productos y su categoría

SELECT
    p.nombre_producto,
    p.precio,
    c.nombre_categoria
FROM productos p
JOIN categorias c
ON p.id_categoria = c.id_categoria;


-- Total de compras por cliente

SELECT
    cl.nombre,
    cl.apellido,
    SUM(v.total) AS total_comprado
FROM ventas v
JOIN clientes cl
ON v.id_cliente = cl.id_cliente
GROUP BY cl.nombre, cl.apellido;


-- Producto más vendido

SELECT
    p.nombre_producto,
    SUM(d.cantidad) AS total_vendido
FROM detalle_ventas d
JOIN productos p
ON d.id_producto = p.id_producto
GROUP BY p.nombre_producto
ORDER BY total_vendido DESC
LIMIT 1;


-- Promedio de ventas

SELECT
    AVG(total) AS promedio_ventas
FROM ventas;


-- Clientes con más de una compra

SELECT
    cl.nombre,
    cl.apellido,
    COUNT(v.id_venta) AS numero_compras
FROM ventas v
JOIN clientes cl
ON v.id_cliente = cl.id_cliente
GROUP BY cl.nombre, cl.apellido
HAVING COUNT(v.id_venta) > 1;


-- Total vendido por categoría

SELECT
    c.nombre_categoria,
    SUM(d.subtotal) AS total_vendido
FROM detalle_ventas d
JOIN productos p
ON d.id_producto = p.id_producto
JOIN categorias c
ON p.id_categoria = c.id_categoria
GROUP BY c.nombre_categoria;


-- Productos con poco stock

SELECT
    nombre_producto,
    stock
FROM productos
WHERE stock < 10;


-- Crear vista de reporte

CREATE VIEW vista_reporte_ventas AS

SELECT
    v.id_venta,
    v.fecha_venta,
    cl.nombre,
    cl.apellido,
    p.nombre_producto,
    d.cantidad,
    d.subtotal
FROM detalle_ventas d
JOIN ventas v
ON d.id_venta = v.id_venta
JOIN productos p
ON d.id_producto = p.id_producto
JOIN clientes cl
ON v.id_cliente = cl.id_cliente;


-- Consultar la vista

SELECT * FROM vista_reporte_ventas;


-- Productos con precio mayor al promedio

SELECT
    nombre_producto,
    precio
FROM productos
WHERE precio >
(
    SELECT AVG(precio)
    FROM productos
);