using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class RecepcionFrutaService
{
    private const string Modulo = "Recepcion";

    private readonly IRecepcionFrutaRepository _recepcionFrutaRepository;
    private readonly IOrdenCorteRepository _ordenCorteRepository;
    private readonly ILoteRepository _loteRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public RecepcionFrutaService(
        IRecepcionFrutaRepository recepcionFrutaRepository,
        IOrdenCorteRepository ordenCorteRepository,
        ILoteRepository loteRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _recepcionFrutaRepository = recepcionFrutaRepository;
        _ordenCorteRepository = ordenCorteRepository;
        _loteRepository = loteRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<RecepcionFrutaDto>> ObtenerAsync()
    {
        var recepciones = await _recepcionFrutaRepository.ObtenerAsync();
        return recepciones.Select(MapearDto).ToList();
    }

    public async Task<RecepcionFrutaDto?> ObtenerPorIdAsync(int id)
        => (await _recepcionFrutaRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public async Task<IReadOnlyList<RecepcionFrutaOrdenCorteDto>> ObtenerDetalleAsync(int recepcionFrutaId)
    {
        var lineas = await _recepcionFrutaRepository.ObtenerDetalleAsync(recepcionFrutaId);
        return lineas.Select(MapearDetalleDto).ToList();
    }

    public Task<RecepcionFrutaReporteDto?> ObtenerParaReporteAsync(int id)
        => _recepcionFrutaRepository.ObtenerParaReporteAsync(id);

    public Task<IReadOnlyList<OrdenCorteDisponibleDto>> ObtenerOrdenesDisponiblesTop100Async()
        => MapearOrdenesDisponiblesAsync(_ordenCorteRepository.ObtenerTop100ParaRecepcionAsync());

    public Task<IReadOnlyList<OrdenCorteDisponibleDto>> BuscarOrdenesDisponiblesAsync(string filtro)
        => MapearOrdenesDisponiblesAsync(_ordenCorteRepository.BuscarParaRecepcionAsync(filtro));

    private static async Task<IReadOnlyList<OrdenCorteDisponibleDto>> MapearOrdenesDisponiblesAsync(
        Task<IReadOnlyList<OrdenCorteDisponible>> tarea)
    {
        var ordenes = await tarea;
        return ordenes
            .Select(o => new OrdenCorteDisponibleDto(o.Id, o.Folio, o.Fecha, o.HuertaNombre, o.CajasEntregadas))
            .ToList();
    }

    public async Task<(int Id, string Folio)> CrearAsync(RecepcionFrutaDto datos)
    {
        var entidad = Validar(datos);

        var resultado = await _recepcionFrutaRepository.InsertarAsync(entidad);

        var creado = (await _recepcionFrutaRepository.ObtenerAsync(resultado.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return resultado;
    }

    public async Task ActualizarAsync(RecepcionFrutaDto datos)
    {
        if (await _loteRepository.RecepcionEstaEnLoteAsync(datos.Id))
        {
            throw new ValidationException("Esta Recepción ya forma parte de un Lote y no se puede editar. Quítala del Lote primero.");
        }

        var anterior = (await _recepcionFrutaRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La recepción de fruta que intentas actualizar ya no existe.");

        var entidad = Validar(datos);
        entidad.Id = datos.Id;
        await _recepcionFrutaRepository.ActualizarAsync(entidad);

        var nuevo = (await _recepcionFrutaRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _recepcionFrutaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _recepcionFrutaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // Agrega una Orden de Corte al detalle de la Recepción. El UNIQUE de SQL en OrdenCorteId ya
    // impide reutilizar la misma orden en dos Recepciones, pero se valida antes también para dar
    // un mensaje claro (mismo criterio que otros módulos del proyecto que respaldan su índice
    // único con una validación previa en el Service).
    public async Task AgregarLineaAsync(int recepcionFrutaId, int ordenCorteId, decimal kilogramos)
    {
        var disponible = await _ordenCorteRepository.EstaDisponibleParaRecepcionAsync(ordenCorteId);
        if (!disponible)
        {
            throw new ValidationException("Esa Orden de Corte ya está registrada en otra Recepción de Fruta.");
        }

        await _recepcionFrutaRepository.InsertarDetalleAsync(new RecepcionFrutaOrdenCorte
        {
            RecepcionFrutaId = recepcionFrutaId,
            OrdenCorteId = ordenCorteId,
            Kilogramos = kilogramos,
        });
    }

    public Task ActualizarLineaAsync(int id, decimal kilogramos)
        => _recepcionFrutaRepository.ActualizarDetalleAsync(id, kilogramos);

    public Task EliminarLineaAsync(int id)
        => _recepcionFrutaRepository.EliminarDetalleAsync(id);

    // Nunca se confía en lo que mande la UI para PesoNeto/CajasDiferencia — se recalculan aquí
    // antes de guardar, mismo criterio que OrdenCorteService.ResolverYValidarAsync.
    private static RecepcionFruta Validar(RecepcionFrutaDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Chofer))
        {
            throw new ValidationException("Captura el nombre del chofer.");
        }

        var pesoNeto = datos.PesoBruto - datos.PesoTara - datos.TaraCajas - datos.PesoMuestra;
        // "Entregadas" es informativo, no participa en la Diferencia — se compara lo que la
        // Orden de Corte decía (Por Entregar) contra lo que efectivamente volvió (Cortadas + Vacías).
        var cajasDiferencia = (short)(datos.CajasPorEntregar - datos.CajasCortadas - datos.CajasRecibidasVacias);

        return new RecepcionFruta
        {
            NoLote = datos.NoLote,
            Fecha = datos.Fecha.Date,
            Chofer = datos.Chofer.Trim(),
            Placas = datos.Placas,
            Observaciones = datos.Observaciones,
            NumeroTicket = datos.NumeroTicket,
            CoprefBico = datos.CoprefBico,
            PesoBruto = datos.PesoBruto,
            PesoTara = datos.PesoTara,
            TaraCajas = datos.TaraCajas,
            PesoMuestra = datos.PesoMuestra,
            PesoNeto = pesoNeto,
            PesoProductor = datos.PesoProductor,
            PorcentajeMateriaSeca = datos.PorcentajeMateriaSeca,
            CajasPorEntregar = datos.CajasPorEntregar,
            CajasEntregadas = datos.CajasEntregadas,
            CajasCortadas = datos.CajasCortadas,
            CajasRecibidasVacias = datos.CajasRecibidasVacias,
            CajasDiferencia = cajasDiferencia,
            CamionDestarado = datos.CamionDestarado,
            TicketPesadaArchivo = datos.TicketPesadaArchivo,
            TicketPesadaNombreArchivo = datos.TicketPesadaNombreArchivo,
        };
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, RecepcionFruta? anterior, RecepcionFruta? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = SerializarParaAuditoria(anterior);
        var valoresNuevos = SerializarParaAuditoria(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    // No se serializa el contenido binario del ticket de báscula al log de auditoría (podría
    // pesar varios MB por registro) — solo se deja constancia de si tenía adjunto y su nombre.
    private static string? SerializarParaAuditoria(RecepcionFruta? r)
    {
        if (r is null)
        {
            return null;
        }

        return JsonSerializer.Serialize(new
        {
            r.Id,
            r.Folio,
            r.NoLote,
            r.Fecha,
            r.Chofer,
            r.Placas,
            r.Observaciones,
            r.NumeroTicket,
            r.CoprefBico,
            r.PesoBruto,
            r.PesoTara,
            r.TaraCajas,
            r.PesoMuestra,
            r.PesoNeto,
            r.PesoProductor,
            r.PorcentajeMateriaSeca,
            r.CajasPorEntregar,
            r.CajasEntregadas,
            r.CajasCortadas,
            r.CajasRecibidasVacias,
            r.CajasDiferencia,
            r.CamionDestarado,
            TicketPesadaAdjunto = r.TicketPesadaArchivo is not null,
            r.TicketPesadaNombreArchivo,
            r.FechaCreacion,
        });
    }

    private static RecepcionFrutaDto MapearDto(RecepcionFruta r) => new(
        r.Id,
        r.Folio,
        r.NoLote,
        r.Fecha,
        r.Chofer,
        r.Placas,
        r.Observaciones,
        r.NumeroTicket,
        r.CoprefBico,
        r.PesoBruto,
        r.PesoTara,
        r.TaraCajas,
        r.PesoMuestra,
        r.PesoNeto,
        r.PesoProductor,
        r.PorcentajeMateriaSeca,
        r.CajasPorEntregar,
        r.CajasEntregadas,
        r.CajasCortadas,
        r.CajasRecibidasVacias,
        r.CajasDiferencia,
        r.CamionDestarado,
        r.TicketPesadaArchivo,
        r.TicketPesadaNombreArchivo,
        r.Huertas);

    private static RecepcionFrutaOrdenCorteDto MapearDetalleDto(RecepcionFrutaOrdenCorte l) => new(
        l.Id,
        l.RecepcionFrutaId,
        l.OrdenCorteId,
        l.OrdenCorteFolio,
        l.HuertaNombre,
        l.CajasCortadas,
        l.Kilogramos);
}
