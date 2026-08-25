using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ListaPrecioFrutaService
{
    private const string Modulo = "Acopio";

    private readonly IListaPrecioFrutaRepository _listaPrecioFrutaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ListaPrecioFrutaService(
        IListaPrecioFrutaRepository listaPrecioFrutaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _listaPrecioFrutaRepository = listaPrecioFrutaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    // Universo de combinaciones capturables (Catalogos.MateriaPrima activas, agrupadas por
    // Categoría+Calibre APEAM) — reemplaza la consulta en vivo a SAP.
    public Task<IReadOnlyList<CombinacionMateriaPrimaDto>> ObtenerCombinacionesActivasAsync()
        => _listaPrecioFrutaRepository.ObtenerCombinacionesActivasAsync();

    public async Task<IReadOnlyList<ListaPrecioFrutaDto>> ObtenerAsync()
    {
        var listas = await _listaPrecioFrutaRepository.ObtenerAsync();
        return listas.Select(MapearDto).ToList();
    }

    private const int MaxDiasPorRango = 366;

    // Un rango (ej. 01/07 al 07/07) ya no se guarda como una sola fila con vigencia — se
    // expande en un registro por día por cada combinación (FechaFin siempre queda NULL). Así
    // la búsqueda por fecha y el traslape (choque exacto Categoría+Calibre+día) quedan simples.
    public async Task GuardarListaAsync(IReadOnlyList<ListaPrecioFrutaDto> filas)
    {
        if (filas.Count == 0)
        {
            throw new ValidationException("No hay combinaciones para guardar");
        }

        foreach (var fila in filas)
        {
            ValidarFila(fila);
        }

        var dias = ObtenerDias(filas[0].FechaInicio, filas[0].FechaFin);
        var instancias = filas
            .SelectMany(fila => dias.Select(dia => fila with { FechaInicio = dia, FechaFin = null }))
            .ToList();

        foreach (var instancia in instancias)
        {
            var existeTraslape = await _listaPrecioFrutaRepository.ExisteTraslapeAsync(
                instancia.CategoriaId, instancia.CalibreApeamId, instancia.FechaInicio, null, instancia.ProductorId);
            if (existeTraslape)
            {
                throw new ValidationException("Ya existe una lista de precios para este día seleccionado.");
            }
        }

        foreach (var instancia in instancias)
        {
            var entidad = MapearEntidad(instancia);
            var id = await _listaPrecioFrutaRepository.InsertarAsync(entidad);

            var creado = (await _listaPrecioFrutaRepository.ObtenerAsync(id)).FirstOrDefault();
            await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);
        }
    }

    public async Task<IReadOnlyList<VigenciaListaPrecioFrutaDto>> ObtenerFechasAsync()
    {
        var vigencias = await _listaPrecioFrutaRepository.ObtenerFechasAsync();
        return vigencias.Select(v => new VigenciaListaPrecioFrutaDto(v.FechaInicio, v.ProductorId, v.ProductorNombre)).ToList();
    }

    // Para Acuerdo de Corte: solo vigencias generales o del productor del acuerdo, dentro de
    // su rango de fechas — nunca listas especiales de otro productor.
    public async Task<IReadOnlyList<VigenciaListaPrecioFrutaDto>> ObtenerFechasPorProductorYRangoAsync(int productorId, DateTime fechaInicio, DateTime fechaFin)
    {
        var vigencias = await _listaPrecioFrutaRepository.ObtenerFechasPorProductorYRangoAsync(productorId, fechaInicio.Date, fechaFin.Date);
        return vigencias.Select(v => new VigenciaListaPrecioFrutaDto(v.FechaInicio, v.ProductorId, v.ProductorNombre)).ToList();
    }

    public async Task<IReadOnlyList<ListaPrecioFrutaDto>> ObtenerPorFechaAsync(DateTime fecha, int? productorId)
    {
        var filas = await _listaPrecioFrutaRepository.ObtenerPorFechaAsync(fecha.Date, productorId);
        return filas.Select(MapearDto).ToList();
    }

    // La fecha (y el productor) de una lista ya guardada no se editan — para "moverla" hay
    // que borrar y volver a capturar desde el form principal. Se audita cada fila borrada
    // por separado.
    public async Task EliminarPorFechaAsync(DateTime fecha, int? productorId)
    {
        var filas = await _listaPrecioFrutaRepository.ObtenerPorFechaAsync(fecha.Date, productorId);
        if (filas.Count == 0)
        {
            throw new ValidationException("No hay una lista de precios guardada para esa fecha.");
        }

        await _listaPrecioFrutaRepository.EliminarPorFechaAsync(fecha.Date, productorId);

        foreach (var fila in filas)
        {
            await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, fila, null);
        }
    }

    private static List<DateTime> ObtenerDias(DateTime fechaInicio, DateTime? fechaFin)
    {
        var fin = (fechaFin ?? fechaInicio).Date;
        var inicio = fechaInicio.Date;

        if (fin < inicio)
        {
            throw new ValidationException("La fecha fin no puede ser anterior a la fecha inicio");
        }

        if ((fin - inicio).Days + 1 > MaxDiasPorRango)
        {
            throw new ValidationException($"El rango no puede superar {MaxDiasPorRango} días");
        }

        var dias = new List<DateTime>();
        for (var dia = inicio; dia <= fin; dia = dia.AddDays(1))
        {
            dias.Add(dia);
        }

        return dias;
    }

    // Solo permite corregir vigencia/precios/activo de un registro ya guardado — la
    // combinación Categoría+Calibre APEAM no cambia. Valida traslape contra otros registros
    // de la misma combinación excluyendo el propio Id.
    public async Task ActualizarAsync(ListaPrecioFrutaDto datos)
    {
        var anterior = (await _listaPrecioFrutaRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La vigencia que intentas actualizar ya no existe.");

        ValidarFila(datos);

        var existeTraslape = await _listaPrecioFrutaRepository.ExisteTraslapeAsync(
            anterior.CategoriaId, anterior.CalibreApeamId, datos.FechaInicio, datos.FechaFin, anterior.ProductorId, datos.Id);
        if (existeTraslape)
        {
            throw new ValidationException("Ya existe otra vigencia de precio que se traslapa para esta combinación");
        }

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _listaPrecioFrutaRepository.ActualizarAsync(entidad);

        var nuevo = (await _listaPrecioFrutaRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, ListaPrecioFruta? anterior, ListaPrecioFruta? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarFila(ListaPrecioFrutaDto fila)
    {
        if (fila.CategoriaId <= 0 || fila.CalibreApeamId <= 0)
        {
            throw new ValidationException("La combinación de Categoría y Calibre APEAM es obligatoria");
        }

        if (fila.Convencional < 0 || fila.Organico < 0 || fila.Nacional < 0)
        {
            throw new ValidationException("Los precios no pueden ser negativos");
        }

        if (fila.FechaFin.HasValue && fila.FechaFin.Value.Date < fila.FechaInicio.Date)
        {
            throw new ValidationException("La fecha fin no puede ser anterior a la fecha inicio");
        }
    }

    private static ListaPrecioFruta MapearEntidad(ListaPrecioFrutaDto datos) => new()
    {
        CategoriaId = datos.CategoriaId,
        CalibreApeamId = datos.CalibreApeamId,
        Convencional = datos.Convencional,
        Organico = datos.Organico,
        Nacional = datos.Nacional,
        FechaInicio = datos.FechaInicio.Date,
        FechaFin = datos.FechaFin?.Date,
        ProductorId = datos.ProductorId,
        Activo = true,
    };

    private static ListaPrecioFrutaDto MapearDto(ListaPrecioFruta l) => new(
        l.Id,
        l.CategoriaId,
        l.CalibreApeamId,
        l.Convencional,
        l.Organico,
        l.Nacional,
        l.FechaInicio,
        l.FechaFin,
        l.Activo,
        l.ProductorId,
        l.CategoriaNombre,
        l.CalibreApeamNombre);
}
