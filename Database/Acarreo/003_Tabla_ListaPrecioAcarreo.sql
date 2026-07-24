USE FrontOne;
GO

-- Lista de precios de acarreo: un renglón por Municipio (cada Municipio pertenece a una Zona),
-- con 3 precios fijos según cantidad de cajas (300/400/500) y una tolerancia de kg faltante.
-- Se captura libremente desde una pantalla tipo grid (no por vigencia de fecha como
-- Acopio.ListaPrecioFruta) — se usará después en la programación de acarreo de una orden de corte.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acarreo.ListaPrecioAcarreo'))
BEGIN
    CREATE TABLE Acarreo.ListaPrecioAcarreo
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acarreo_ListaPrecioAcarreo PRIMARY KEY,
        MunicipioId             INT                 NOT NULL CONSTRAINT FK_Acarreo_ListaPrecioAcarreo_Municipio REFERENCES Catalogos.Municipio(Id),
        ZonaId                  INT                 NOT NULL CONSTRAINT FK_Acarreo_ListaPrecioAcarreo_Zona REFERENCES Acarreo.Zona(Id),
        Precio300               DECIMAL(18,2)       NOT NULL,
        Precio400               DECIMAL(18,2)       NOT NULL,
        Precio500               DECIMAL(18,2)       NOT NULL,
        ToleranciaKgFaltante    INT                 NOT NULL,
        Activo                  BIT                 NOT NULL CONSTRAINT DF_Acarreo_ListaPrecioAcarreo_Activo DEFAULT (1),
        CONSTRAINT UQ_Acarreo_ListaPrecioAcarreo_Municipio UNIQUE (MunicipioId)
    );
END
GO
