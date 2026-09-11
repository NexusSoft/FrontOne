-- Soporte para el mapa de Huertas en FrontOne.Android (submódulo Acopio > Huertas).
-- Excepción documentada a la regla general de que los catálogos se quedan en escritorio:
-- desde el celular solo se puede CONSULTAR huertas (con coordenadas) y CORREGIR su pin
-- de ubicación — el resto de los datos de la huerta sigue siendo de solo lectura y se
-- edita únicamente desde HuertaEditarForm en escritorio. Ver contexto/catalogos.md.

-- Catalogos.Huerta tiene un índice filtrado (RegistroSagarpa único cuando no es NULL) —
-- cualquier UPDATE contra la tabla exige QUOTED_IDENTIFIER ON en la sesión que CREÓ el
-- procedimiento (queda grabado como metadata del SP, no del caller en tiempo de
-- ejecución), igual que ya hace 011_Schema_Huerta.sql.
SET QUOTED_IDENTIFIER ON;
GO

-- Carga inicial del mapa, sin filtro — mismo patrón "TOP 100 por defecto" que ya exige
-- CLAUDE.md para buscadores embebidos de catálogos grandes (ProductoresForm/HuertasForm).
CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ObtenerTop100ConCoordenadas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 h.Id, h.Nombre, h.RegistroSagarpa, h.Latitud, h.Longitud,
           h.ProductorId, p.NombreProductor AS ProductorNombre
    FROM Catalogos.Huerta h
    JOIN Catalogos.Productor p ON p.Id = h.ProductorId
    WHERE h.Latitud IS NOT NULL AND h.Longitud IS NOT NULL AND h.Activo = 1
    ORDER BY h.Nombre;
END
GO

-- Búsqueda por Registro SAGARPA, Nombre de huerta o nombre del Productor — mínimo 2
-- caracteres aplicado del lado de la app (mismo criterio que sp_Huerta_Buscar), TOP 500.
CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_BuscarConCoordenadas
    @Filtro NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500 h.Id, h.Nombre, h.RegistroSagarpa, h.Latitud, h.Longitud,
           h.ProductorId, p.NombreProductor AS ProductorNombre
    FROM Catalogos.Huerta h
    JOIN Catalogos.Productor p ON p.Id = h.ProductorId
    WHERE h.Latitud IS NOT NULL AND h.Longitud IS NOT NULL AND h.Activo = 1
      AND (h.Nombre LIKE '%' + @Filtro + '%'
           OR h.RegistroSagarpa LIKE '%' + @Filtro + '%'
           OR p.NombreProductor LIKE '%' + @Filtro + '%')
    ORDER BY h.Nombre;
END
GO

-- Update ligero y dedicado — solo Latitud/Longitud, nunca el resto de la fila (mismo
-- criterio que Recepcion.sp_RecepcionFruta_ActualizarNoLote). El rango -90..90/-180..180
-- se valida en la app (Android) antes de llamar aquí, igual que HuertaService.cs en
-- escritorio — este SP no revalida el rango, solo persiste.
CREATE OR ALTER PROCEDURE Catalogos.sp_Huerta_ActualizarUbicacion
    @Id INT,
    @Latitud DECIMAL(9,6),
    @Longitud DECIMAL(9,6)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.Huerta
    SET Latitud = @Latitud, Longitud = @Longitud
    WHERE Id = @Id;
END
GO
