CREATE DATABASE Payflow;
go

use Payflow;
go

-- Tabla Usuarios (solo usuarios normales)
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    DNI CHAR(8) NOT NULL UNIQUE,
    CorreoElectronico VARCHAR(100) NOT NULL UNIQUE,
    ContraseñaHash VARCHAR(255) NOT NULL,
    EstadoUsuario VARCHAR(10) NOT NULL DEFAULT 'Activo', -- 'Activo', 'Bloqueado', 'Inactivo'
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);

-- Tabla Administradores (independiente de la tabla Usuarios)
CREATE TABLE Administradores (
    AdministradorID INT IDENTITY(1,1) PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    CorreoElectronico VARCHAR(100) NOT NULL UNIQUE,
    ContraseñaHash VARCHAR(255) NOT NULL, -- Contraseña encriptada
    EstadoAdministrador VARCHAR(10) NOT NULL DEFAULT 'Activo', -- 'Activo', 'Bloqueado'
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    EsSuperAdmin BIT NOT NULL DEFAULT 0 -- Para diferenciar administradores con más privilegios
);

-- Tabla Cuentas (una cuenta por usuario), con EstadoCuenta
CREATE TABLE Cuentas (
    CuentaID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL UNIQUE,
    NumeroCuenta CHAR(10) NOT NULL UNIQUE,
    Saldo DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    EstadoCuenta VARCHAR(15) NOT NULL DEFAULT 'Activo', -- Estado de la cuenta (Activo, Bloqueado, etc.)
    CONSTRAINT FK_Cuentas_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla Transacciones, con CuentaDestinoID para transferencias entre cuentas dentro de la misma plataforma
CREATE TABLE Transacciones (
    TransaccionID INT IDENTITY(1,1) PRIMARY KEY,
    CuentaID INT NOT NULL,
    TipoTransaccion VARCHAR(15) NOT NULL CHECK (TipoTransaccion IN ('Deposito','Retiro','Transferencia')),
    Monto DECIMAL(12,2) NOT NULL CHECK (Monto > 0),
    FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
    Estado VARCHAR(15) NOT NULL DEFAULT 'Pendiente' CHECK (Estado IN ('Pendiente','Aceptado','Rechazado')),
    NumeroOperacion VARCHAR(50) NULL,
    Banco VARCHAR(100) NULL,
    RutaVoucher VARCHAR(255) NULL,
    ComentariosAdmin VARCHAR(500) NULL,
    Comentario VARCHAR(500) NULL, -- Comentario opcional
    CuentaDestinoID INT NULL, -- Para transferencias a cuentas dentro de la plataforma
    IPOrigen VARCHAR(45) NULL,
    Ubicacion VARCHAR(100) NULL,
    CONSTRAINT FK_Transacciones_Cuentas FOREIGN KEY (CuentaID) REFERENCES Cuentas(CuentaID),
    CONSTRAINT FK_Transacciones_CuentaDestino FOREIGN KEY (CuentaDestinoID) REFERENCES Cuentas(CuentaID)
);

-- Tabla Notificaciones, relacionada con Transacciones
CREATE TABLE Notificaciones (
    NotificacionID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL, -- Usuario que recibe la notificación
    TransaccionID INT NULL, -- Transacción asociada a la notificación
    TipoNotificacion VARCHAR(15) NOT NULL CHECK (TipoNotificacion IN ('Deposito','Retiro','Alerta')),
    Mensaje TEXT NOT NULL,
    FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
    Estado VARCHAR(10) NOT NULL DEFAULT 'No Leido' CHECK (Estado IN ('Leido','No Leido')),
    CONSTRAINT FK_Notificaciones_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    CONSTRAINT FK_Notificaciones_Transacciones FOREIGN KEY (TransaccionID) REFERENCES Transacciones(TransaccionID)
);

-- Tabla HistorialValidaciones
CREATE TABLE HistorialValidaciones (
    ValidacionID INT IDENTITY(1,1) PRIMARY KEY,
    TransaccionID INT NOT NULL,
    AdministradorID INT NOT NULL,
    FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
    TipoValidacion VARCHAR(10) NOT NULL CHECK (TipoValidacion IN ('Manual','Automatica')),
    Resultado VARCHAR(15) NOT NULL CHECK (Resultado IN ('Aceptado','Rechazado','Pendiente')),
    Comentarios TEXT NULL,
    CONSTRAINT FK_Validaciones_Transacciones FOREIGN KEY (TransaccionID) REFERENCES Transacciones(TransaccionID),
    CONSTRAINT FK_Validaciones_Admin FOREIGN KEY (AdministradorID) REFERENCES Administradores(AdministradorID)
);

-- Índices para mejorar las consultas de las transacciones
CREATE INDEX IX_Transacciones_FechaEstado ON Transacciones(FechaHora, Estado);

INSERT INTO Usuarios (Nombres, Apellidos, DNI, CorreoElectronico, ContraseñaHash, EstadoUsuario, FechaRegistro)
VALUES
('Juan', 'Pérez', '12345678', 'juan.perez@example.com', 'hashed_password_1', 'Activo', GETDATE()),
('María', 'González', '23456789', 'maria.gonzalez@example.com', 'hashed_password_2', 'Activo', GETDATE()),
('Carlos', 'López', '34567890', 'carlos.lopez@example.com', 'hashed_password_3', 'Activo', GETDATE()),
('Ana', 'Martínez', '45678901', 'ana.martinez@example.com', 'hashed_password_4', 'Inactivo', GETDATE()),
('Luis', 'Sánchez', '56789012', 'luis.sanchez@example.com', 'hashed_password_5', 'Activo', GETDATE()),
('Elena', 'Díaz', '67890123', 'elena.diaz@example.com', 'hashed_password_6', 'Bloqueado', GETDATE()),
('Pedro', 'Álvarez', '78901234', 'pedro.alvarez@example.com', 'hashed_password_7', 'Activo', GETDATE()),
('Laura', 'Ramírez', '89012345', 'laura.ramirez@example.com', 'hashed_password_8', 'Activo', GETDATE()),
('Miguel', 'Fernández', '90123456', 'miguel.fernandez@example.com', 'hashed_password_9', 'Inactivo', GETDATE()),
('Sofía', 'Torres', '01234567', 'sofia.torres@example.com', 'hashed_password_10', 'Activo', GETDATE());

INSERT INTO Administradores (Nombres, Apellidos, CorreoElectronico, ContraseñaHash, EstadoAdministrador, FechaRegistro, EsSuperAdmin)
VALUES
('Jorge', 'Martínez', 'jorge.martinez@admin.com', 'hashed_password_1', 'Activo', GETDATE(), 1),
('Marta', 'Gómez', 'marta.gomez@admin.com', 'hashed_password_2', 'Activo', GETDATE(), 0),
('Pedro', 'Serrano', 'pedro.serrano@admin.com', 'hashed_password_3', 'Activo', GETDATE(), 0),
('Sandra', 'Vázquez', 'sandra.vazquez@admin.com', 'hashed_password_4', 'Inactivo', GETDATE(), 0),
('Ricardo', 'Sánchez', 'ricardo.sanchez@admin.com', 'hashed_password_5', 'Activo', GETDATE(), 1),
('Lucía', 'Jiménez', 'lucia.jimenez@admin.com', 'hashed_password_6', 'Activo', GETDATE(), 0),
('Daniel', 'Morales', 'daniel.morales@admin.com', 'hashed_password_7', 'Activo', GETDATE(), 1),
('Carmen', 'Ríos', 'carmen.rios@admin.com', 'hashed_password_8', 'Bloqueado', GETDATE(), 0),
('Juan', 'Córdoba', 'juan.cordoba@admin.com', 'hashed_password_9', 'Activo', GETDATE(), 1),
('Laura', 'López', 'laura.lopez@admin.com', 'hashed_password_10', 'Inactivo', GETDATE(), 0);

INSERT INTO Cuentas (UsuarioID, NumeroCuenta, Saldo, EstadoCuenta)
VALUES
(1, '2001000001', 1500.00, 'Activo'),
(2, '2001000002', 2200.50, 'Activo'),
(3, '2001000003', 500.00, 'Inactivo'),
(4, '2001000004', 0.00, 'Bloqueado'),
(5, '2001000005', 1000.00, 'Activo'),
(6, '2001000006', 3200.00, 'Activo'),
(7, '2001000007', 1100.00, 'Activo'),
(8, '2001000008', 300.00, 'Activo'),
(9, '2001000009', 850.75, 'Inactivo'),
(10, '2001000010', 5000.00, 'Activo');

INSERT INTO Transacciones (CuentaID, TipoTransaccion, Monto, FechaHora, Estado, NumeroOperacion, Banco, RutaVoucher, ComentariosAdmin, Comentario, CuentaDestinoID, IPOrigen, Ubicacion)
VALUES
(1, 'Deposito', 1000.00, GETDATE(), 'Aceptado', 'OP1234', 'Banco A', 'voucher1.pdf', 'Depósito aceptado', 'Depósito realizado correctamente', NULL, '192.168.1.1', 'Lima'),
(2, 'Retiro', 500.00, GETDATE(), 'Pendiente', 'OP1235', 'Banco B', 'voucher2.pdf', 'Retiro pendiente', 'Monto para retiro', NULL, '192.168.1.2', 'Arequipa'),
(3, 'Transferencia', 200.00, GETDATE(), 'Aceptado', 'OP1236', 'Banco C', 'voucher3.pdf', 'Transferencia completada', 'Transferencia a cuenta 2001000004', 4, '192.168.1.3', 'Cusco'),
(4, 'Deposito', 3000.00, GETDATE(), 'Aceptado', 'OP1237', 'Banco A', 'voucher4.pdf', 'Depósito aceptado', 'Depósito de grandes fondos', NULL, '192.168.1.4', 'Trujillo'),
(5, 'Retiro', 150.00, GETDATE(), 'Pendiente', 'OP1238', 'Banco D', 'voucher5.pdf', 'Retiro pendiente', 'Retiro en proceso', NULL, '192.168.1.5', 'Puno'),
(6, 'Transferencia', 500.00, GETDATE(), 'Aceptado', 'OP1239', 'Banco E', 'voucher6.pdf', 'Transferencia completada', 'Transferencia realizada', 7, '192.168.1.6', 'Piura'),
(7, 'Deposito', 2000.00, GETDATE(), 'Pendiente', 'OP1240', 'Banco F', 'voucher7.pdf', 'Depósito pendiente', 'Esperando validación de depósito', NULL, '192.168.1.7', 'Lima'),
(8, 'Retiro', 120.00, GETDATE(), 'Aceptado', 'OP1241', 'Banco G', 'voucher8.pdf', 'Retiro aceptado', 'Retiro realizado sin problemas', NULL, '192.168.1.8', 'Iquitos'),
(9, 'Transferencia', 450.00, GETDATE(), 'Rechazado', 'OP1242', 'Banco H', 'voucher9.pdf', 'Transferencia rechazada', 'Fondos insuficientes', 10, '192.168.1.9', 'Tacna'),
(10, 'Deposito', 6000.00, GETDATE(), 'Aceptado', 'OP1243', 'Banco I', 'voucher10.pdf', 'Depósito exitoso', 'Depósito de alto monto aceptado', NULL, '192.168.1.10', 'Lima');

INSERT INTO Notificaciones (UsuarioID, TransaccionID, TipoNotificacion, Mensaje, FechaHora, Estado)
VALUES
(1, 1, 'Deposito', 'Depósito de 1000.00 aceptado', GETDATE(), 'No Leido'),
(2, 2, 'Retiro', 'Retiro de 500.00 en proceso', GETDATE(), 'No Leido'),
(3, 3, 'Deposito', 'Transferencia de 200.00 a cuenta 2001000004 realizada', GETDATE(), 'No Leido'),
(4, 4, 'Deposito', 'Depósito de 3000.00 aceptado', GETDATE(), 'No Leido'),
(5, 5, 'Retiro', 'Retiro de 150.00 pendiente', GETDATE(), 'No Leido'),
(6, 6, 'Retiro', 'Transferencia de 500.00 a cuenta 2001000007 realizada', GETDATE(), 'No Leido'),
(7, 7, 'Deposito', 'Depósito de 2000.00 pendiente', GETDATE(), 'No Leido'),
(8, 8, 'Retiro', 'Retiro de 120.00 aceptado', GETDATE(), 'No Leido'),
(9, 9, 'Deposito', 'Transferencia de 450.00 rechazada debido a fondos insuficientes', GETDATE(), 'No Leido'),
(10, 10, 'Deposito', 'Depósito de 6000.00 exitoso', GETDATE(), 'No Leido');

INSERT INTO HistorialValidaciones (TransaccionID, AdministradorID, FechaHora, TipoValidacion, Resultado, Comentarios)
VALUES
(1, 1, GETDATE(), 'Manual', 'Aceptado', 'Depósito validado correctamente'),
(2, 2, GETDATE(), 'Automatica', 'Pendiente', 'Retiro en espera de validación'),
(3, 3, GETDATE(), 'Manual', 'Rechazado', 'Fondos insuficientes para la transferencia'),
(4, 4, GETDATE(), 'Automatica', 'Aceptado', 'Depósito aprobado tras verificación automática'),
(5, 5, GETDATE(), 'Manual', 'Pendiente', 'Retiro pendiente de verificación de cuenta'),
(6, 6, GETDATE(), 'Automatica', 'Aceptado', 'Transferencia verificada automáticamente'),
(7, 7, GETDATE(), 'Manual', 'Rechazado', 'Depósito rechazado por error en comprobante'),
(8, 8, GETDATE(), 'Automatica', 'Aceptado', 'Retiro procesado correctamente'),
(9, 9, GETDATE(), 'Manual', 'Rechazado', 'Transferencia rechazada por fondos insuficientes'),
(10, 10, GETDATE(), 'Automatica', 'Aceptado', 'Depósito completado y aprobado');