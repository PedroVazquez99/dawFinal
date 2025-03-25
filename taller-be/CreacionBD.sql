

-- IMPORTANTE

-- Realizar generacion de la BBDD con la migracion
    --> Update-Database 



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


-- Reservas

INSERT INTO TaskList (Color, Fecha, Nombre, Visible, UsuarioId, ServicioId) VALUES
('#FF5733', '2025-04-01 10:30:00', 'Carlos Rodríguez', 'S', 1, 2),
('#33FF57', '2025-04-01 11:45:00', 'María Fernández', 'S', 2, 3),
('#3357FF', '2025-04-01 14:00:00', 'Juan Pérez', 'S', 3, 1),
('#FF33A1', '2025-04-01 16:15:00', 'Ana López', 'S', 4, 4),
('#A133FF', '2025-04-01 18:30:00', 'Pedro Gómez', 'S', 5, 2),
('#FF8C33', '2025-04-02 10:00:00', 'Sofía Martínez', 'S', 6, 3),
('#33FFF5', '2025-04-02 12:45:00', 'David Herrera', 'S', 7, 1)