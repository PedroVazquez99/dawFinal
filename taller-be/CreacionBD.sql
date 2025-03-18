

-- Crear tabla TaskList
CREATE TABLE TaskList (
    id NUMERIC(18, 0) IDENTITY(1,1) NOT NULL PRIMARY KEY,
    color VARCHAR(10) NOT NULL DEFAULT '#FFFFFF',
    fecha DATETIME NOT NULL,
    nombre VARCHAR(250) NOT NULL,
    visible CHAR(1) NOT NULL DEFAULT 'S'
);

-- Crear tabla ItemTask
CREATE TABLE ItemTask (
    id NUMERIC(18, 0) IDENTITY(1,1) NOT NULL PRIMARY KEY,
    caduca DATETIME NULL,
    fecha DATETIME NOT NULL,
    lista NUMERIC(18, 0) NOT NULL,
    terminada CHAR(1) NOT NULL DEFAULT 'N',
    texto VARCHAR(250) NOT NULL,
    visible CHAR(1) NOT NULL DEFAULT 'S',
    CONSTRAINT FK_Tasks_Lists FOREIGN KEY (lista) REFERENCES TaskList(id)
);

-- Crear tabla Usuarios
CREATE TABLE Usuarios (
    id NUMERIC(18, 0) IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    email VARCHAR(250) NOT NULL UNIQUE,
    password_hash VARCHAR(250) NOT NULL,
    rol VARCHAR(10) NOT NULL CHECK (rol IN ('admin', 'user')) DEFAULT 'user',
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE()
);

