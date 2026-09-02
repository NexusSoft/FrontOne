namespace FrontOne.Domain.Entities;

// Encabezado del proceso de Reempaque. KilosAProcesar/KilosProcesados nunca se capturan: los
// mueven los SPs de entrada/salida (Produccion.sp_Reempaque_AgregarPalletOrigen y
// sp_PalletDetalle_InsertarDesdeReempaque/sp_PalletDetalle_Eliminar/sp_Reempaque_CrearNeutro).
public class Reempaque
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public TimeSpan HoraCreacion { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public byte Estatus { get; set; }
    public decimal KilosAProcesar { get; set; }
    public decimal KilosProcesados { get; set; }
    public decimal Diferencia { get; set; }
    public DateTime? FechaCierre { get; set; }
    public DateTime FechaCreacionRegistro { get; set; }
}
