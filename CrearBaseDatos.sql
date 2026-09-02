-- ============================================================
-- Desafío Práctico #2 - Sistema de Gestión de Eventos
-- Script IDEMPOTENTE de creación de base de datos y tablas.
-- Se puede ejecutar manualmente (SSMS) o automáticamente al
-- iniciar la API (ver Eventos.DAL/DatabaseInitializer.cs):
-- en ambos casos, si la BD/tablas ya existen, no hace nada
-- destructivo (no se pierden datos ya insertados vía la API).
-- ============================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GestionEventos')
BEGIN
    CREATE DATABASE GestionEventos;
END
GO

USE GestionEventos;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Eventos') AND type = 'U')
BEGIN
    CREATE TABLE Eventos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Fecha DATETIME NOT NULL,
        Lugar NVARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Participantes') AND type = 'U')
BEGIN
    CREATE TABLE Participantes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        EventoId INT NOT NULL,
        CONSTRAINT FK_Participantes_Eventos FOREIGN KEY (EventoId) REFERENCES Eventos(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Organizadores') AND type = 'U')
BEGIN
    CREATE TABLE Organizadores (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL,
        Cargo NVARCHAR(50) NOT NULL,
        EventoId INT NOT NULL,
        CONSTRAINT FK_Organizadores_Eventos FOREIGN KEY (EventoId) REFERENCES Eventos(Id) ON DELETE CASCADE
    );
END
GO

-- Datos de prueba: solo se insertan la primera vez (tabla vacía)
IF NOT EXISTS (SELECT 1 FROM Eventos)
BEGIN
    INSERT INTO Eventos (Nombre, Fecha, Lugar) VALUES
    ('Conferencia de Tecnologia 2026', '2026-10-15', 'Centro de Convenciones UDB'),
    ('Feria de Emprendimiento', '2026-11-02', 'Campus Antiguo Cuscatlan'),
    ('Hackathon Estudiantil', '2026-09-20', 'Laboratorio de Innovacion');
END
GO

IF NOT EXISTS (SELECT 1 FROM Participantes)
BEGIN
    INSERT INTO Participantes (Nombre, Email, EventoId) VALUES
    ('Ana Martinez', 'ana.martinez@udb.edu.sv', 1),
    ('Carlos Lopez', 'carlos.lopez@udb.edu.sv', 1),
    ('Maria Hernandez', 'maria.hernandez@udb.edu.sv', 2);
END
GO

IF NOT EXISTS (SELECT 1 FROM Organizadores)
BEGIN
    INSERT INTO Organizadores (Nombre, Cargo, EventoId) VALUES
    ('Emerson Cartagena', 'Coordinador Academico', 1),
    ('Laura Pineda', 'Encargada de Logistica', 2),
    ('Jose Ramirez', 'Coordinador de Innovacion', 3);
END
GO
