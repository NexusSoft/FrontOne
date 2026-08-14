using System.Text;

namespace FrontOne.Shared.Utils;

// Implementación del "Voice Pick Code" estándar de la industria (PTI/GS1, whitepaper de
// YottaMark/HarvestMark, de uso libre) — usado en la etiqueta de Caja del módulo Etiquetado para
// que un operador de almacén identifique una caja sin escanear el código de barras.
//
// Algoritmo (no se persiste en BD, se calcula al vuelo antes de imprimir):
// 1. PlainText = GTIN (14 dígitos) + Lote + Fecha (yyMMdd, si se proporciona), sin separadores.
// 2. CRC-16 ANSI (polinomio reflejado 0xA001, el mismo algoritmo Modbus/ANSI estándar) sobre los
//    bytes ASCII del PlainText.
// 3. VoiceCode = crc % 10000 (4 dígitos, con ceros a la izquierda).
// 4. Low = 2 dígitos menos significativos (se imprimen grandes); High = 2 dígitos más
//    significativos (se imprimen chicos).
public static class VoicePickCodeCalculator
{
    private const ushort Polynomial = 0xA001;
    private static readonly ushort[] Tabla = ConstruirTabla();

    public static (string Low, string High) Calcular(string gtin, string lote, DateTime? fecha = null)
    {
        var plainText = string.Concat(gtin, lote, fecha.HasValue ? fecha.Value.ToString("yyMMdd") : string.Empty);
        var crc = ComputarChecksum(Encoding.ASCII.GetBytes(plainText));
        var voiceCode = (crc % 10000).ToString("0000");

        return (Low: voiceCode[2..], High: voiceCode[..2]);
    }

    private static ushort ComputarChecksum(byte[] bytes)
    {
        ushort crc = 0;
        foreach (var b in bytes)
        {
            var index = (byte)(crc ^ b);
            crc = (ushort)((crc >> 8) ^ Tabla[index]);
        }

        return crc;
    }

    private static ushort[] ConstruirTabla()
    {
        var tabla = new ushort[256];
        for (ushort i = 0; i < tabla.Length; ++i)
        {
            ushort value = 0;
            ushort temp = i;
            for (byte j = 0; j < 8; ++j)
            {
                if (((value ^ temp) & 0x0001) != 0)
                {
                    value = (ushort)((value >> 1) ^ Polynomial);
                }
                else
                {
                    value >>= 1;
                }

                temp >>= 1;
            }

            tabla[i] = value;
        }

        return tabla;
    }
}
