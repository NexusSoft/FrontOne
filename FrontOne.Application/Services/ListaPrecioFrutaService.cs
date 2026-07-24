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
    private const string PrefijoItemFruta = "MP";

    private readonly IListaPrecioFrutaRepository _listaPrecioFrutaRepository;
    private readonly ISapItemRepository _sapItemRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ListaPrecioFrutaService(
        IListaPrecioFrutaRepository listaPrecioFrutaRepository,
        ISapItemRepository sapItemRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _listaPrecioFrutaRepository = listaPrecioFrutaRepository;
        _sapItemRepository = sapItemRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    // Trae de SAP únicamente ItemCode/ItemName de los productos de fruta (prefijo MP) — los
    // precios (Lista1/2/3) se capturan a mano en FrontOne, SAP no los provee para este módulo.
    public Task<IReadOnlyList<SapItemDto>> ObtenerItemsFrutaDeSapAsync(CancellationToken cancellationToken = default)
        => _sapItemRepository.ObtenerPorPrefijoAsync(PrefijoItemFruta, cancellationToken);

    public async Task<IReadOnlyList<ListaPrecioFrutaDto>> ObtenerAsync()
    {
        var listas = await _listaPrecioFrutaRepository.ObtenerAsync();
        return listas.Select(MapearDto).ToList();
    }

    private const int MaxDiasPorRango = 366;

    // Un rango (ej. 01/07 al 07/07) ya no se guarda como una sola fila con vigencia — se
    // expande en un registro por día por cada producto (FechaFin siempre queda NULL). Así la
    // búsqueda por fecha y el traslape (choque exacto ItemCode+día) quedan simples.
    public async Task GuardarListaAsync(IReadOnlyList<ListaPrecioFrutaDto> filas)
    {
        if (filas.Count == 0)
        {
            throw new ValidationException("No hay productos para guardar");
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
                instancia.ItemCode, instancia.FechaInicio, null, instancia.ProductorId, instancia.VariedadId);
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
        return vigencias.Select(v => new VigenciaListaPrecioFrutaDto(v.FechaInicio, v.ProductorId, v.ProductorNombre, v.VariedadId, v.VariedadNombre)).ToList();
    }

    // Para Acuerdo de Corte: solo vigencias generales o del productor del acuerdo, dentro de
    // su rango de fechas — nunca listas especiales de otro productor.
    public async Task<IReadOnlyList<VigenciaListaPrecioFrutaDto>> ObtenerFechasPorProductorYRangoAsync(int productorId, DateTime fechaInicio, DateTime fechaFin)
    {
        var vigencias = await _listaPrecioFrutaRepository.ObtenerFechasPorProductorYRangoAsync(productorId, fechaInicio.Date, fechaFin.Date);
        return vigencias.Select(v => new VigenciaListaPrecioFrutaDto(v.FechaInicio, v.ProductorId, v.ProductorNombre, v.VariedadId, v.VariedadNombre)).ToList();
    }

    public async Task<IReadOnlyList<ListaPrecioFrutaDto>> ObtenerPorFechaAsync(DateTime fecha, int? productorId, int? variedadId)
    {
        var filas = await _listaPrecioFrutaRepository.ObtenerPorFechaAsync(fecha.Date, productorId, variedadId);
        return filas.Select(MapearDto).ToList();
    }

    // La fecha (y el productor) de una lista ya guardada no se editan — para "moverla" hay
    // que borrar y volver a capturar desde el form principal. Se audita cada fila borrada
    // por separado.
    public async Task EliminarPorFechaAsync(DateTime fecha, int? productorId, int? variedadId)
    {
        var filas = await _listaPrecioFrutaRepository.ObtenerPorFechaAsync(fecha.Date, productorId, variedadId);
        if (filas.Count == 0)
        {
            throw new ValidationException("No hay una lista de precios guardada para esa fecha.");
        }

        await _listaPrecioFrutaRepository.EliminarPorFechaAsync(fecha.Date, productorId, variedadId);

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

    // Solo permite corregir vigencia/precios/activo de un registro ya guardado — ItemCode/
    // ItemName no cambian (vienen de SAP). Valida traslape contra otros registros del mismo
    // ItemCode excluyendo el propio Id.
    public async Task ActualizarAsync(ListaPrecioFrutaDto datos)
    {
        var anterior = (await _listaPrecioFrutaRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La vigencia que intentas actualizar ya no existe.");

        ValidarFila(datos);

        var existeTraslape = await _listaPrecioFrutaRepository.ExisteTraslapeAsync(
            anterior.ItemCode, datos.FechaInicio, datos.FechaFin, anterior.ProductorId, anterior.VariedadId, datos.Id);
        if (existeTraslape)
        {
            throw new ValidationException($"Ya existe otra vigencia de precio que se traslapa para {anterior.ItemCode}");
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
        if (string.IsNullOrWhiteSpace(fila.ItemCode))
        {
            throw new ValidationException("El ItemCode del producto es obligatorio");
        }

        if (fila.VariedadId is null or <= 0)
        {
            throw new ValidationException($"Selecciona la variedad para {fila.ItemCode}");
        }

        if (fila.Lista1 < 0 || fila.Lista2 < 0 || fila.Lista3 < 0)
        {
            throw new ValidationException($"Los precios de {fila.ItemCode} no pueden ser negativos");
        }

        if (fila.FechaFin.HasValue && fila.FechaFin.Value.Date < fila.FechaInicio.Date)
        {
            throw new ValidationException($"La fecha fin de {fila.ItemCode} no puede ser anterior a la fecha inicio");
        }
    }

    private static ListaPrecioFruta MapearEntidad(ListaPrecioFrutaDto datos) => new()
    {
        ItemCode = datos.ItemCode.Trim(),
        ItemName = datos.ItemName.Trim(),
        Lista1 = datos.Lista1,
        Lista2 = datos.Lista2,
        Lista3 = datos.Lista3,
        FechaInicio = datos.FechaInicio.Date,
        FechaFin = datos.FechaFin?.Date,
        ProductorId = datos.ProductorId,
        VariedadId = datos.VariedadId,
        Activo = true,
    };

    private static ListaPrecioFrutaDto MapearDto(ListaPrecioFruta l) => new(
        l.Id,
        l.ItemCode,
        l.ItemName,
        l.Lista1,
        l.Lista2,
        l.Lista3,
        l.FechaInicio,
        l.FechaFin,
        l.Activo,
        l.ProductorId,
        l.VariedadId);
}
