namespace FrontOne.WinForms.Reports.Controles;

// Subconjunto de simbologías de TECIT.TBarCode.BarcodeType que el proyecto expone en el
// Diseñador de Reportes — se amplía conforme se necesiten más (Rss, DataMatrix con opciones
// GS1, etc.), no se expone el enum completo de TECIT directo en el Property Grid.
public enum TipoSimbologiaBarcode
{
    Code128,
    Gs1_128,
    QRCode,
    DataMatrix,
    Pdf417,
}
