USE FrontOne;
GO

-- Variedad se promueve a campo duro del encabezado del Lote (antes solo se hubiera mostrado
-- derivada al vuelo, como Huerta/Productor). Se persiste porque el futuro módulo de Costos
-- (precios a la banda) va a vincular cada Lote a una única Lista de Precio (Convencional/
-- Orgánico/Nacional, Acopio.AcuerdoCorte.ListaPrecioNumero) a través de esta Variedad — ver
-- contexto/lotes.md para el detalle completo de la decisión.
ALTER TABLE Lotes.Lote ADD VariedadId INT NULL;
GO

ALTER TABLE Lotes.Lote
    ADD CONSTRAINT FK_Lotes_Lote_Variedad FOREIGN KEY (VariedadId) REFERENCES Acopio.Variedad(Id);
GO

-- Backfill de Lotes ya existentes (si los hubiera) — misma cadena de joins que ya usaba
-- sp_Lote_Obtener para HuertaNombre/ProductorNombre: se toma la Variedad de la Orden de Corte de
-- la primera Recepción del Lote.
UPDATE l
SET l.VariedadId = primera.VariedadId
FROM Lotes.Lote l
OUTER APPLY (
    SELECT TOP 1 oc.VariedadId
    FROM Lotes.LoteRecepcion det
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    WHERE det.LoteId = l.Id
    ORDER BY det.Id
) AS primera
WHERE l.VariedadId IS NULL;
GO
