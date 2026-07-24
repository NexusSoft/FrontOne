USE FrontOne;
GO

-- Reinicia el contador IDENTITY a 1 (RESEED a 0) únicamente en las tablas que están
-- vacías — en una tabla con filas, reiniciar el contador provoca choque de llave primaria
-- en el siguiente INSERT (el contador volvería a intentar Id=1, que ya existiría o
-- rompería el orden). Las tablas con datos se listan como "omitida", sin tocarlas.
DECLARE @Tabla NVARCHAR(261), @Columna SYSNAME, @Filas BIGINT, @Sql NVARCHAR(MAX);

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT QUOTENAME(s.name) + '.' + QUOTENAME(t.name), c.name
    FROM sys.identity_columns c
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
    ORDER BY s.name, t.name;

OPEN cur;
FETCH NEXT FROM cur INTO @Tabla, @Columna;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @Sql = N'SELECT @FilasOut = COUNT(*) FROM ' + @Tabla;
    EXEC sp_executesql @Sql, N'@FilasOut BIGINT OUTPUT', @FilasOut = @Filas OUTPUT;

    IF @Filas = 0
    BEGIN
        DBCC CHECKIDENT (@Tabla, RESEED, 0);
        PRINT @Tabla + ': reiniciada (vacía) — el próximo INSERT usará Id = 1';
    END
    ELSE
    BEGIN
        PRINT @Tabla + ': omitida (' + CAST(@Filas AS VARCHAR(20)) + ' filas existentes)';
    END

    FETCH NEXT FROM cur INTO @Tabla, @Columna;
END

CLOSE cur;
DEALLOCATE cur;
GO
