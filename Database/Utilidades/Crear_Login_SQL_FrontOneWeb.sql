USE master;
GO

-- Login SQL dedicado para FrontOne.Web (nunca 'sa', nunca db_owner). Permisos mínimos:
-- EXECUTE sobre los schemas de negocio y nada más — ni SELECT/INSERT/UPDATE/DELETE directo sobre
-- tablas (todo acceso pasa por stored procedures vía Dapper/SqlRepositoryBase), ni permisos de
-- administración de servidor/base. Ejecutar UNA VEZ contra el servidor de producción antes de
-- desplegar el sitio; en desarrollo se puede seguir usando 'sa' localmente si se prefiere.
--
-- Cambiar '<PASSWORD_AQUI>' por una contraseña fuerte generada aparte — NUNCA dejarla en este
-- archivo ni commitearla. Después de crear el login, la contraseña real se configura solo como
-- variable de entorno Sql__Password en el App Pool de IIS (ver docs/despliegue-web-iis.md), nunca
-- en appsettings.json.
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'frontone_web')
BEGIN
    CREATE LOGIN frontone_web WITH PASSWORD = '<PASSWORD_AQUI>', CHECK_POLICY = ON;
END
GO

USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'frontone_web')
BEGIN
    CREATE USER frontone_web FOR LOGIN frontone_web;
END
GO

-- EXECUTE sobre cada schema de negocio (todos los que existen hoy en FrontOne). Un schema nuevo
-- que se agregue más adelante necesita su propia línea aquí — no hay wildcard por diseño, así el
-- otorgamiento siempre es explícito y auditable.
GRANT EXECUTE ON SCHEMA::Seguridad   TO frontone_web;
GRANT EXECUTE ON SCHEMA::Auditoria  TO frontone_web;
GRANT EXECUTE ON SCHEMA::Catalogos  TO frontone_web;
GRANT EXECUTE ON SCHEMA::Acopio     TO frontone_web;
GRANT EXECUTE ON SCHEMA::Acarreo    TO frontone_web;
GRANT EXECUTE ON SCHEMA::Configuracion TO frontone_web;
GRANT EXECUTE ON SCHEMA::Recepcion  TO frontone_web;
GRANT EXECUTE ON SCHEMA::Lotes      TO frontone_web;
GRANT EXECUTE ON SCHEMA::Almacenes  TO frontone_web;
GRANT EXECUTE ON SCHEMA::Produccion TO frontone_web;
GRANT EXECUTE ON SCHEMA::Etiquetado TO frontone_web;
GRANT EXECUTE ON SCHEMA::Gastos     TO frontone_web;
GO
