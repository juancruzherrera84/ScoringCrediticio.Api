-- Activo las FK (en SQLite vienen desactivadas por defecto)
PRAGMA foreign_keys = ON;

-- ==========================
-- TABLA: Cliente
-- ==========================
CREATE TABLE IF NOT EXISTS Cliente (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre          TEXT NOT NULL,
    Apellido        TEXT NOT NULL,
    Documento       TEXT NOT NULL UNIQUE,
    FechaNacimiento TEXT NULL,          -- Guardar en formato ISO 8601 (YYYY-MM-DD)
    Email           TEXT NULL,
    Telefono        TEXT NULL
);

-- ==========================
-- TABLA: DatosFinancieros
-- (relación 1:1 con Cliente)
-- ==========================
CREATE TABLE IF NOT EXISTS DatosFinancieros (
    Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    ClienteId               INTEGER NOT NULL UNIQUE,   -- 1:1 con Cliente
    IngresosMensuales       NUMERIC NOT NULL DEFAULT 0,
    TieneCuentaBancaria     INTEGER NOT NULL DEFAULT 0,   -- 0 = false, 1 = true
    TieneTarjetaCredito     INTEGER NOT NULL DEFAULT 0,
    AntiguedadLaboralMeses  INTEGER NOT NULL DEFAULT 0,
    TieneDeudasRegistradas  INTEGER NOT NULL DEFAULT 0,
    MontoDeudaAproximado    NUMERIC NOT NULL DEFAULT 0,
    CantidadTarjetasCredito INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id) ON DELETE CASCADE
);

-- ==========================
-- TABLA: Solicitud
-- (N por Cliente)
-- ==========================
CREATE TABLE IF NOT EXISTS Solicitud (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    ClienteId       INTEGER NOT NULL,
    TipoProducto    INTEGER NOT NULL,                -- enum TipoProducto
    MontoSolicitado NUMERIC NOT NULL,
    PlazoMeses      INTEGER NOT NULL,
    FechaCreacion   TEXT NOT NULL DEFAULT (datetime('now')),
    Estado          INTEGER NOT NULL,                -- enum EstadoSolicitud
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id) ON DELETE CASCADE
);

-- ==========================
-- TABLA: ResultadoScoring
-- (1:1 con Solicitud)
-- ==========================
CREATE TABLE IF NOT EXISTS ResultadoScoring (
    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
    SolicitudId    INTEGER NOT NULL UNIQUE,          -- 1:1
    Puntaje        REAL NOT NULL,
    NivelRiesgo    INTEGER NOT NULL,                -- enum NivelRiesgo
    Observaciones  TEXT NULL,
    FechaCalculo   TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (SolicitudId) REFERENCES Solicitud(Id) ON DELETE CASCADE
);

-- ==========================
-- ÍNDICES ÚTILES
-- ==========================

-- Búsqueda rápida por documento de cliente
CREATE INDEX IF NOT EXISTS IX_Cliente_Documento
    ON Cliente (Documento);

-- Búsqueda rápida de solicitudes por cliente
CREATE INDEX IF NOT EXISTS IX_Solicitud_ClienteId
    ON Solicitud (ClienteId);

-- Búsqueda rápida de resultado por solicitud
CREATE INDEX IF NOT EXISTS IX_ResultadoScoring_SolicitudId
    ON ResultadoScoring (SolicitudId);
