CREATE DATABASE IF NOT EXISTS gestion_recetas;
USE gestion_recetas;

CREATE TABLE Recetas (
    receta_id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    tiempo_preparacion INT NOT NULL,
    dificultad VARCHAR(20) NOT NULL,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT ck_dificultad CHECK (dificultad IN ('Fácil', 'Media', 'Difícil'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE Ingredientes (
    ingrediente_id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    cantidad DECIMAL(8, 2) NOT NULL,
    unidad VARCHAR(20) NOT NULL,
    calorias INT,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE RecetaIngredientes (
    receta_ingrediente_id INT AUTO_INCREMENT PRIMARY KEY,
    receta_id INT NOT NULL,
    ingrediente_id INT NOT NULL,
    cantidad_utilizada DECIMAL(8, 2) NOT NULL,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (receta_id) REFERENCES Recetas(receta_id) ON DELETE CASCADE,
    FOREIGN KEY (ingrediente_id) REFERENCES Ingredientes(ingrediente_id) ON DELETE CASCADE,
    UNIQUE KEY uk_receta_ingrediente (receta_id, ingrediente_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO Recetas (nombre, descripcion, tiempo_preparacion, dificultad) VALUES
('Pasta Carbonara', 'Pasta carbonara italiana', 20, 'Fácil'),
('Pollo al Horno', 'Pollo asado con limón', 45, 'Media'),
('Ensalada César', 'Ensalada fresca', 10, 'Fácil');

INSERT INTO Ingredientes (nombre, cantidad, unidad, calorias) VALUES
('Pasta', 500, 'gr', 131),
('Huevo', 3, 'unidad', 155),
('Jamón Serrano', 200, 'gr', 161),
('Queso Parmesano', 100, 'gr', 392),
('Pollo', 1500, 'gr', 165);

INSERT INTO RecetaIngredientes (receta_id, ingrediente_id, cantidad_utilizada) VALUES
(1, 1, 400),
(1, 2, 3),
(1, 3, 150),
(1, 4, 80),
(2, 5, 1500),
(3, 1, 200),
(3, 4, 50);
