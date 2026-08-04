USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Almacenes')
BEGIN
    EXEC('CREATE SCHEMA Almacenes');
END
GO

-- Bitácora de movimientos de caja de campo (entradas/salidas). El saldo (existencia) se calcula
-- siempre como SUM(Entrada) - SUM(Salida), nunca se guarda un total aparte que se pueda desfasar.
-- OrigenId es referencia lógica (sin FK) a Acopio.OrdenCorte o Recepcion.RecepcionFruta según
-- OrigenModulo — cruzar dos schemas con FK dura complicaría el borrado del registro origen sin
-- aportar nada, ya que el movimiento histórico no debe desaparecer si se borra el origen.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Almacenes.MovimientoCajaCampo'))
BEGIN
    CREATE TABLE Almacenes.MovimientoCajaCampo
    (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Fecha           DATE NOT NULL,
        CajaCampoId     INT NOT NULL CONSTRAINT FK_Almacenes_MovimientoCajaCampo_CajaCampo FOREIGN KEY REFERENCES Catalogos.CajaCampo (Id),
        TipoMovimiento  NVARCHAR(20) NOT NULL CONSTRAINT CK_Almacenes_MovimientoCajaCampo_TipoMovimiento CHECK (TipoMovimiento IN ('Entrada', 'Salida')),
        Cantidad        SMALLINT NOT NULL CONSTRAINT CK_Almacenes_MovimientoCajaCampo_Cantidad CHECK (Cantidad > 0),
        OrigenModulo    NVARCHAR(20) NOT NULL CONSTRAINT CK_Almacenes_MovimientoCajaCampo_OrigenModulo CHECK (OrigenModulo IN ('OrdenCorte', 'Recepcion', 'Manual')),
        OrigenId        INT NULL,
        Observaciones   NVARCHAR(500) NULL,
        Usuario         NVARCHAR(100) NOT NULL,
        FechaCreacion   DATETIME2 NOT NULL CONSTRAINT DF_Almacenes_MovimientoCajaCampo_FechaCreacion DEFAULT (SYSUTCDATETIME())
    );

    CREATE INDEX IX_Almacenes_MovimientoCajaCampo_Origen ON Almacenes.MovimientoCajaCampo (OrigenModulo, OrigenId);
END
GO
