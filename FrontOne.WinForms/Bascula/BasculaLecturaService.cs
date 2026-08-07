using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Bascula;

// Lectura del peso desde una báscula conectada por puerto serie. Vive en WinForms y no en
// Application porque System.IO.Ports es una dependencia de plataforma (Application no puede
// depender de ella sin volverse específica de Windows).
//
// El parser es genérico a propósito: cada modelo de báscula manda una trama distinta
// ("ST,GS,   12.34 kg", "  12.34\r\n", "+0012.34"...). Si Configuracion.Bascula.PatronLectura
// trae una expresión regular con grupo de captura, se usa ese grupo; si no, se toma el primer
// número que aparezca en la trama. Se afina contra la báscula real cuando se conecte.
public static class BasculaLecturaService
{
    private const int TiempoEsperaMilisegundos = 3000;

    // Fallback cuando no hay patrón configurado: primer número de la trama, con signo y
    // decimales opcionales.
    private static readonly Regex PrimerNumero = new(@"[-+]?\d+(?:[.,]\d+)?", RegexOptions.Compiled);

    public static IReadOnlyList<string> ObtenerPuertosDisponibles()
        => SerialPort.GetPortNames().OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToList();

    // Abre el puerto configurado, lee una trama y devuelve el peso. Lanza InvalidOperationException
    // con un mensaje ya listo para mostrar al usuario cuando la configuración está incompleta, el
    // puerto no responde o la trama no trae un número reconocible.
    public static decimal TomarLectura(ConfiguracionBasculaDto configuracion)
    {
        if (string.IsNullOrWhiteSpace(configuracion.Puerto))
        {
            throw new InvalidOperationException(
                "La báscula no está configurada: captura el puerto en Sistema › Configuración de báscula.");
        }

        var trama = LeerTrama(configuracion);
        if (string.IsNullOrWhiteSpace(trama))
        {
            throw new InvalidOperationException(
                $"La báscula no envió datos por el puerto {configuracion.Puerto}. Verifica que esté encendida y conectada.");
        }

        return ExtraerPeso(trama, configuracion.PatronLectura);
    }

    // Público para que "Probar lectura" de la pantalla de configuración muestre la trama cruda
    // tal cual la mandó la báscula — es el dato que hace falta para armar el patrón.
    public static string LeerTrama(ConfiguracionBasculaDto configuracion)
    {
        using var puerto = new SerialPort(
            configuracion.Puerto,
            configuracion.BaudRate,
            (Parity)configuracion.Parity,
            configuracion.DataBits,
            (StopBits)configuracion.StopBits)
        {
            ReadTimeout = TiempoEsperaMilisegundos,
            NewLine = "\r\n",
        };

        try
        {
            puerto.Open();
            // ReadExisting no espera: se usa ReadLine para bloquear hasta que la báscula mande una
            // trama completa. Si el modelo no termina con \r\n, el timeout deja lo que haya llegado.
            try
            {
                return puerto.ReadLine();
            }
            catch (TimeoutException)
            {
                return puerto.ReadExisting();
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException(
                $"El puerto {configuracion.Puerto} ya está siendo usado por otro programa.");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException(
                $"No se pudo abrir el puerto {configuracion.Puerto}: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(
                $"La configuración del puerto {configuracion.Puerto} no es válida: {ex.Message}");
        }
    }

    public static decimal ExtraerPeso(string trama, string? patron)
    {
        string? textoNumero = null;

        if (!string.IsNullOrWhiteSpace(patron))
        {
            var coincidencia = Regex.Match(trama, patron);
            if (coincidencia.Success)
            {
                // Si el patrón trae grupo de captura se usa el grupo 1; si no, la coincidencia completa.
                textoNumero = coincidencia.Groups.Count > 1 && coincidencia.Groups[1].Success
                    ? coincidencia.Groups[1].Value
                    : coincidencia.Value;
            }
        }
        else
        {
            var coincidencia = PrimerNumero.Match(trama);
            if (coincidencia.Success)
            {
                textoNumero = coincidencia.Value;
            }
        }

        if (string.IsNullOrWhiteSpace(textoNumero))
        {
            throw new InvalidOperationException(
                $"No se pudo interpretar el peso de la báscula. Trama recibida: \"{trama.Trim()}\".");
        }

        // La báscula puede mandar coma decimal según su configuración regional: se normaliza a
        // punto antes de convertir con InvariantCulture.
        var normalizado = textoNumero.Replace(',', '.');
        if (!decimal.TryParse(normalizado, NumberStyles.Number, CultureInfo.InvariantCulture, out var peso))
        {
            throw new InvalidOperationException(
                $"No se pudo interpretar el peso de la báscula. Trama recibida: \"{trama.Trim()}\".");
        }

        return peso;
    }
}
