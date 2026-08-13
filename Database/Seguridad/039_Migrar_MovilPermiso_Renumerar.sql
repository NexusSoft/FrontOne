USE FrontOne;
GO

-- Migración de una sola vez: mueve los permisos de las 9 "pantallas" móviles (AccesoMovil del
-- módulo Seguridad + las 8 del módulo AplicacionMovil) de Seguridad.Pantalla/Permiso a la tabla
-- nueva Seguridad.MovilPermiso, y renumera los Id que quedan en Seguridad.Pantalla para que no
-- quede hueco. Identifica las pantallas a mover DINÁMICAMENTE (por ModuloId/Nombre, nunca por un
-- rango de Id fijo — el rango exacto varía por ambiente). Guardado con IF EXISTS: si ya se corrió
-- (el módulo AplicacionMovil ya no existe), una segunda ejecución no hace nada.
IF EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'AplicacionMovil')
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @Liberados TABLE (Id INT PRIMARY KEY);

        INSERT INTO @Liberados (Id)
        SELECT p.Id
        FROM Seguridad.Pantalla p
        INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
        WHERE m.Nombre = 'AplicacionMovil' OR (m.Nombre = 'Seguridad' AND p.Nombre = 'AccesoMovil');

        -- Migrar los permisos ya otorgados (4 acciones por pantalla) a Seguridad.MovilPermiso.
        INSERT INTO Seguridad.MovilPermiso (RolId, PantallaCodigo, Consultar, Crear, Modificar, Eliminar)
        SELECT
            pe.RolId,
            p.Nombre,
            MAX(CASE WHEN a.Nombre = 'Consultar' THEN 1 ELSE 0 END),
            MAX(CASE WHEN a.Nombre = 'Crear' THEN 1 ELSE 0 END),
            MAX(CASE WHEN a.Nombre = 'Modificar' THEN 1 ELSE 0 END),
            MAX(CASE WHEN a.Nombre = 'Eliminar' THEN 1 ELSE 0 END)
        FROM Seguridad.Permiso pe
        INNER JOIN @Liberados lib ON lib.Id = pe.PantallaId
        INNER JOIN Seguridad.Pantalla p ON p.Id = pe.PantallaId
        INNER JOIN Seguridad.Accion a ON a.Id = pe.AccionId
        GROUP BY pe.RolId, p.Nombre;

        -- Liberar la FK y borrar las 9 filas de Pantalla + el módulo AplicacionMovil (ya sin
        -- Pantalla que lo referencie).
        DELETE pe FROM Seguridad.Permiso pe INNER JOIN @Liberados lib ON lib.Id = pe.PantallaId;
        DELETE p FROM Seguridad.Pantalla p INNER JOIN @Liberados lib ON lib.Id = p.Id;
        DELETE FROM Seguridad.Modulo WHERE Nombre = 'AplicacionMovil';

        -- Renumerar: cada Pantalla que quedó después del hueco baja su Id tantos lugares como
        -- Ids liberados haya por debajo de él, uno por uno, en orden ascendente. Se desactiva la FK
        -- (NOCHECK) durante el proceso porque el orden DELETE-fila-vieja → INSERT-fila-nueva → UPDATE
        -- de Permiso deja momentáneamente PantallaId apuntando a un Id que aún no existe; hacerlo al
        -- revés (INSERT primero) choca con el UNIQUE(ModuloId, Nombre) porque la fila vieja con el
        -- mismo Nombre todavía no se borró. Al final se reactiva CON CHECK, que revalida todo.
        ALTER TABLE Seguridad.Permiso NOCHECK CONSTRAINT FK_Seguridad_Permiso_Pantalla;

        DECLARE @Pendientes TABLE (Id INT PRIMARY KEY);
        INSERT INTO @Pendientes (Id)
        SELECT Id FROM Seguridad.Pantalla WHERE Id > (SELECT MIN(Id) FROM @Liberados);

        DECLARE @IdViejo INT, @IdNuevo INT, @ModuloId INT, @Nombre NVARCHAR(100), @Descripcion NVARCHAR(300);

        WHILE EXISTS (SELECT 1 FROM @Pendientes)
        BEGIN
            SELECT TOP 1 @IdViejo = Id FROM @Pendientes ORDER BY Id ASC;

            SELECT @ModuloId = ModuloId, @Nombre = Nombre, @Descripcion = Descripcion
            FROM Seguridad.Pantalla WHERE Id = @IdViejo;

            SET @IdNuevo = @IdViejo - (SELECT COUNT(*) FROM @Liberados WHERE Id < @IdViejo);

            DELETE FROM Seguridad.Pantalla WHERE Id = @IdViejo;

            SET IDENTITY_INSERT Seguridad.Pantalla ON;
            INSERT INTO Seguridad.Pantalla (Id, ModuloId, Nombre, Descripcion)
            VALUES (@IdNuevo, @ModuloId, @Nombre, @Descripcion);
            SET IDENTITY_INSERT Seguridad.Pantalla OFF;

            UPDATE Seguridad.Permiso SET PantallaId = @IdNuevo WHERE PantallaId = @IdViejo;

            DELETE FROM @Pendientes WHERE Id = @IdViejo;
        END

        ALTER TABLE Seguridad.Permiso WITH CHECK CHECK CONSTRAINT FK_Seguridad_Permiso_Pantalla;

        DECLARE @MaxIdFinal INT = ISNULL((SELECT MAX(Id) FROM Seguridad.Pantalla), 0);
        DBCC CHECKIDENT ('Seguridad.Pantalla', RESEED, @MaxIdFinal);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
