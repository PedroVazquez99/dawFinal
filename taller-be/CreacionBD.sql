

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

-- Usuarios 

INSERT INTO Usuarios (nombre, email, password_hash, rol)
VALUES (
    'user',
    'user@example.com',
    CONVERT(VARCHAR(250), HASHBYTES('SHA2_256', '1234'), 2), -- Convierte a hexadecimal
    'user'
);

INSERT INTO Usuarios (nombre, email, password_hash, rol)
VALUES (
    'admin',
    'admin@example.com',
    CONVERT(VARCHAR(250), HASHBYTES('SHA2_256', '1234'), 2),
    'admin'
);

-- Servicios

INSERT INTO Servicio (Nombre, Descripcion, Precio, Duracion)
VALUES
    ('Corte', 'Corte de cabello clásico.', 15.00, '00:30:00'),
    ('Corte + Barba', 'Corte de cabello y arreglo de barba.', 25.00, '01:00:00'),
    ('Arreglo de Barba', 'Recorte y arreglo de barba.', 10.00, '00:30:00');
    ('Corte infantil', 'Corte de cabello especializado para niños.', 15.00, '00:30'),
    ('Tratamiento capilar', 'Tratamiento profundo para la hidratación y reparación del cabello.', 40.00, '01:00'),
    ('Alisado permanente', 'Alisado profesional para mantener el cabello liso por meses.', 120.00, '02:30'),
    ('Peinado de novia', 'Peinado exclusivo para bodas y eventos especiales.', 80.00, '01:30'),
    ('Extensiones de cabello', 'Colocación de extensiones para agregar longitud y volumen.', 150.00, '03:00'),
    ('Coloración completa', 'Aplicación de tinte en todo el cabello para un nuevo look.', 70.00, '02:00');