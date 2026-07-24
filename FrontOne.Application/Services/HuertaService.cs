using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class HuertaService
{
    private const string Modulo = "Catalogos";

    private readonly IHuertaRepository _huertaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public HuertaService(IHuertaRepository huertaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _huertaRepository = huertaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<HuertaDto>> ObtenerAsync()
    {
        var huertas = await _huertaRepository.ObtenerAsync();
        return huertas.Select(MapearDto).ToList();
    }

    public async Task<IReadOnlyList<HuertaDto>> ObtenerPorProductorAsync(int productorId)
    {
        var huertas = await _huertaRepository.ObtenerAsync(productorId: productorId);
        return huertas.Select(MapearDto).ToList();
    }

    // Búsqueda para el picker: filtro server-side, máximo 500 resultados.
    public Task<IReadOnlyList<HuertaBusquedaDto>> BuscarAsync(string filtro)
        => _huertaRepository.BuscarAsync(filtro);

    // Carga inicial del picker (sin filtro) para que el grid no se vea vacío al abrir.
    public Task<IReadOnlyList<HuertaBusquedaDto>> ObtenerTop100Async()
        => _huertaRepository.ObtenerTop100Async();

    public async Task<HuertaDto?> ObtenerPorIdAsync(int id)
    {
        var huerta = (await _huertaRepository.ObtenerAsync(id)).FirstOrDefault();
        return huerta is null ? null : MapearDto(huerta);
    }

    // Navegación del catálogo completo por Id (orden de creación) — seek puntual,
    // nunca carga todas las huertas a memoria. Usado por Inicio/Anterior/Siguiente/Fin.
    public async Task<HuertaDto?> ObtenerPrimeroAsync()
    {
        var huerta = await _huertaRepository.ObtenerPrimeroAsync();
        return huerta is null ? null : MapearDto(huerta);
    }

    public async Task<HuertaDto?> ObtenerUltimoAsync()
    {
        var huerta = await _huertaRepository.ObtenerUltimoAsync();
        return huerta is null ? null : MapearDto(huerta);
    }

    public async Task<HuertaDto?> ObtenerSiguienteAsync(int id)
    {
        var huerta = await _huertaRepository.ObtenerSiguienteAsync(id);
        return huerta is null ? null : MapearDto(huerta);
    }

    public async Task<HuertaDto?> ObtenerAnteriorAsync(int id)
    {
        var huerta = await _huertaRepository.ObtenerAnteriorAsync(id);
        return huerta is null ? null : MapearDto(huerta);
    }

    public async Task<int> CrearAsync(HuertaDto datos)
    {
        await ValidarAsync(datos, esNuevo: true);

        var id = await _huertaRepository.InsertarAsync(MapearEntidad(datos));

        var creado = (await _huertaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(HuertaDto datos)
    {
        var anterior = (await _huertaRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La huerta que intentas actualizar ya no existe.");

        // El duplicado de SAGARPA solo se valida si el usuario lo cambió — un registro
        // existente que conserva su propio SAGARPA siempre debe poder actualizarse.
        var sagarpaCambio = !string.Equals(
            anterior.RegistroSagarpa?.Trim(),
            datos.RegistroSagarpa?.Trim(),
            StringComparison.OrdinalIgnoreCase);

        await ValidarAsync(datos, esNuevo: false, validarSagarpa: sagarpaCambio);

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _huertaRepository.ActualizarAsync(entidad);

        var nuevo = (await _huertaRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _huertaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _huertaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private async Task ValidarAsync(HuertaDto datos, bool esNuevo, bool validarSagarpa = true)
    {
        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            throw new ValidationException("El nombre de la huerta es obligatorio");
        }

        if (datos.ProductorId <= 0)
        {
            throw new ValidationException("El productor es obligatorio");
        }

        if (validarSagarpa && !string.IsNullOrWhiteSpace(datos.RegistroSagarpa))
        {
            var idExcluir = esNuevo ? null : (int?)datos.Id;
            var existe = await _huertaRepository.ExisteRegistroSagarpaAsync(datos.RegistroSagarpa.Trim(), idExcluir);
            if (existe)
            {
                throw new ValidationException($"Ya existe otra huerta con el Registro SAGARPA '{datos.RegistroSagarpa.Trim()}'");
            }
        }

        if (datos.Latitud is < -90 or > 90)
        {
            throw new ValidationException("La latitud debe estar entre -90 y 90 grados");
        }

        if (datos.Longitud is < -180 or > 180)
        {
            throw new ValidationException("La longitud debe estar entre -180 y 180 grados");
        }
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Huerta? anterior, Huerta? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static Huerta MapearEntidad(HuertaDto datos) => new()
    {
        Nombre = datos.Nombre.Trim(),
        ProductorId = datos.ProductorId,
        Ubicacion = datos.Ubicacion,
        PoblacionId = datos.PoblacionId,
        MunicipioId = datos.MunicipioId,
        EstadoId = datos.EstadoId,
        ProductoId = datos.ProductoId,
        EncargadoNombre = datos.EncargadoNombre,
        EncargadoTelefono = datos.EncargadoTelefono,
        Observaciones = datos.Observaciones,
        RegistroSagarpa = string.IsNullOrWhiteSpace(datos.RegistroSagarpa) ? null : datos.RegistroSagarpa.Trim(),
        CertificadoGlobalGap = datos.CertificadoGlobalGap,
        RegistroFda = datos.RegistroFda,
        NumeroGlobalGap = datos.NumeroGlobalGap,
        Superficie = datos.Superficie,
        Altura = datos.Altura,
        NumeroArboles = datos.NumeroArboles,
        EdadArboles = datos.EdadArboles,
        SistemaRiegoId = datos.SistemaRiegoId,
        PorcentajeMecanizacion = datos.PorcentajeMecanizacion,
        StatusHuertaId = datos.StatusHuertaId,
        FechaCambioStatus = datos.FechaCambioStatus,
        FechaVencimiento = datos.FechaVencimiento,
        Latitud = datos.Latitud,
        Longitud = datos.Longitud,
        Activo = datos.Activo,
    };

    private static HuertaDto MapearDto(Huerta h) => new(
        h.Id,
        h.Nombre,
        h.ProductorId,
        h.Ubicacion,
        h.PoblacionId,
        h.MunicipioId,
        h.EstadoId,
        h.ProductoId,
        h.EncargadoNombre,
        h.EncargadoTelefono,
        h.Observaciones,
        h.RegistroSagarpa,
        h.CertificadoGlobalGap,
        h.RegistroFda,
        h.NumeroGlobalGap,
        h.Superficie,
        h.Altura,
        h.NumeroArboles,
        h.EdadArboles,
        h.SistemaRiegoId,
        h.PorcentajeMecanizacion,
        h.StatusHuertaId,
        h.FechaCambioStatus,
        h.FechaVencimiento,
        h.Latitud,
        h.Longitud,
        h.Activo,
        h.AniosEnStatusActual,
        h.DiasParaVencimiento);
}
