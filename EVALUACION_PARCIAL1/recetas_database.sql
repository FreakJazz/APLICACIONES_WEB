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

CREATE INDEX idx_recetas_nombre ON Recetas(nombre);
CREATE INDEX idx_ingredientes_nombre ON Ingredientes(nombre);
CREATE INDEX idx_receta_ingredientes_receta ON RecetaIngredientes(receta_id);
CREATE INDEX idx_receta_ingredientes_ingrediente ON RecetaIngredientes(ingrediente_id);
