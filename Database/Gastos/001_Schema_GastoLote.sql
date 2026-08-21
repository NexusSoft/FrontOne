USE FrontOne;
GO

-- Módulo Gastos: liquidación de costos por Lote (Fruta, Cosecha, Acarreo) una vez que su
-- Corrida ya está Finalizada (Produccion.Corrida.Estatus = 2) — antes de eso el Lote no
-- aparece en el listado de Gastos, no se puede costear.
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Gastos')
BEGIN
    EXEC('CREATE SCHEMA Gastos');
END
GO

-- Encabezado: 1 fila por Lote costeado. Folio/Huerta/Registro Sagarpa/Fecha/Peso/Tipo de
-- Pago/Tipo de Corte/Variedad/Precio Acordado se derivan en vivo del Lote/Corrida/AcuerdoCorte
-- (mismo criterio que Produccion.sp_Corrida_Obtener) — no se duplican aquí. Lo único que se
-- persiste es la vigencia de Costo Estimado, elegida independiente de la del Acuerdo de Corte.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Gastos.GastoLote'))
BEGIN
    CREATE TABLE Gastos.GastoLote (
        Id                                   INT IDENTITY PRIMARY KEY,
        LoteId                               INT NOT NULL UNIQUE REFERENCES Lotes.Lote(Id),
        CostoEstimadoListaPrecioFecha        DATE NULL,
        CostoEstimadoListaPrecioProductorId  INT NULL REFERENCES Catalogos.Productor(Id),
        CostoEstimadoListaPrecioNumero       TINYINT NULL, -- 1/2/3 -> Convencional/Organico/Nacional
        FechaCreacion                        DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO
