namespace FrontOne.Domain.DTOs;

public record ContenedorDto(
    int Id,
    string Folio,
    DateTime Fecha,
    int SapDocEntry,
    int SapDocNum,
    string? FolioFronterra,
    string CardCode,
    string CardName,
    string? Observaciones,
    DateTime FechaCreacionRegistro,
    int TotalPallets);

// Fila del grid izquierdo del Tab Embarque: un pallet ya cargado al contenedor.
public record ContenedorPalletDto(
    int ContenedorPalletId,
    int NoRegistro,
    int PalletId,
    string PalletFolio,
    int Posicion,
    decimal? Temperatura,
    int Cajas,
    decimal Kilogramos);

// Fila del grid derecha-abajo: resumen agrupado por Calibre de Exportación.
public record ContenedorResumenCalibreDto(
    string CalibreExportacion,
    int TotalPallets,
    int TotalCajas,
    decimal TotalKilogramos);

// Cajas/Kilogramos ya surtidos de un producto SAP, para cruzar contra la Cantidad Cajas del pedido.
public record ContenedorSurtidoDto(
    string CodigoSap,
    int CajasSurtidas,
    decimal KilogramosSurtidos);

// Fila del grid del Tab Pedido — combina la línea del pedido SAP con lo ya surtido en pallets.
public record ContenedorPedidoLineaDto(
    string CodigoProducto,
    string DescripcionProducto,
    decimal CantidadCajas,
    string Presentacion,
    decimal? Pallet,
    decimal Kilogramos,
    decimal PorcentajeSurtido,
    string Status);

// Fila plana (pallet+línea de lote) para los 7 reportes de carga de contenedor — ver
// Embarques.sp_Contenedor_ObtenerCargaParaReporte y FrontOne.WinForms/Reports/ReporteContenedorComun.cs.
public record ContenedorCargaReporteLineaDto(
    int Posicion,
    string PalletFolio,
    DateTime PalletFechaCreacion,
    string LoteFolio,
    DateTime LoteFecha,
    string ProductoDescripcion,
    string ProductoPresentacionTexto,
    string? CalibreCodigoExterno,
    string? MarcaNombre,
    int Cajas,
    decimal Kilogramos,
    string? HuertaNombre,
    string? RegistroSagarpa,
    string? NumeroGlobalGap,
    bool CertificadoGlobalGap,
    string? PoblacionNombre,
    string? Municipio,
    string? ProductorNombre,
    decimal? Temperatura);

// Fila del buscador de pallets disponibles para embarcar.
public record PalletDisponibleEmbarqueDto(
    int Id,
    string Folio,
    DateTime FechaCreacion,
    byte Estatus,
    int? TotalCajas,
    decimal? TotalKilogramos,
    bool EsMixto,
    string ProductoDescripcion);
