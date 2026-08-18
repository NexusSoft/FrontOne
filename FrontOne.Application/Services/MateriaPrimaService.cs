using System.Text.Json;
using FrontOne.Application.Validators;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public record ResultadoSincronizacionMateriaPrima(int Nuevos, int Actualizados, int Reactivados, int Desactivados, int Errores);

public class MateriaPrimaService
{
    private const string Modulo = "Catalogos";

    // Grupo de artículos "Materia Prima" en SAP Business One (ItemGroups.GroupName) — el mismo
    // grupo "MP" que ProductoTerminadoService ya usa (en vivo, sin persistir) para llenar el
    // LookUpEdit de Materia Prima en ProductoTerminadoEditarForm. Aquí sí se persiste como
    // catálogo local. Si el nombre del grupo cambia en el ambiente de producción, hay que
    // actualizar esta constante.
    private const string GrupoMateriaPrimaNombre = "MP";

    private readonly IMateriaPrimaRepository _materiaPrimaRepository;
    private readonly ISapItemRepository _sapItemRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly MateriaPrimaValidator _validator;

    public MateriaPrimaService(
        IMateriaPrimaRepository materiaPrimaRepository,
        ISapItemRepository sapItemRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider,
        MateriaPrimaValidator validator)
    {
        _materiaPrimaRepository = materiaPrimaRepository;
        _sapItemRepository = sapItemRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
        _validator = validator;
    }

    public async Task<IReadOnlyList<MateriaPrimaDto>> ObtenerAsync()
    {
        var materiasPrima = await _materiaPrimaRepository.ObtenerAsync();
        return materiasPrima.Select(MapearDto).ToList();
    }

    public async Task<MateriaPrimaDto?> ObtenerPorIdAsync(int id)
    {
        var materiasPrima = await _materiaPrimaRepository.ObtenerAsync(id);
        return materiasPrima.Select(MapearDto).FirstOrDefault();
    }

    // Carga inicial del listado (sin filtro) para que el grid no se vea vacío al abrir.
    public async Task<IReadOnlyList<MateriaPrimaDto>> ObtenerTop1000Async()
    {
        var materiasPrima = await _materiaPrimaRepository.ObtenerTop1000Async();
        return materiasPrima.Select(MapearDto).ToList();
    }

    // Búsqueda para el listado: filtro server-side, máximo 500 resultados.
    public async Task<IReadOnlyList<MateriaPrimaDto>> BuscarAsync(string filtro)
    {
        var materiasPrima = await _materiaPrimaRepository.BuscarAsync(filtro);
        return materiasPrima.Select(MapearDto).ToList();
    }

    // Trae de SAP las materias primas vigentes (grupo "MP") y sincroniza contra la tabla local:
    // agrega las que faltan (con Categoría/Calibre pendientes de captura), actualiza la
    // descripción si cambió en SAP, reactiva/desactiva por Valid, y nunca borra ningún registro
    // (una materia prima desactivada conserva toda su captura de negocio por si SAP la reactiva
    // después).
    public async Task<ResultadoSincronizacionMateriaPrima> SincronizarConSapAsync(CancellationToken cancellationToken = default)
    {
        var nuevos = 0;
        var actualizados = 0;
        var reactivados = 0;
        var desactivados = 0;
        var errores = 0;

        IReadOnlyList<SapProductoTerminadoDto> materiasPrimaSap;
        try
        {
            materiasPrimaSap = await _sapItemRepository.ObtenerPorGrupoAsync(GrupoMateriaPrimaNombre, cancellationToken);
        }
        catch (SapException)
        {
            // No se pudo consultar SAP (grupo no encontrado, Service Layer caído, etc.):
            // se reporta como un error de sincronización sin tumbar el listado local.
            return new ResultadoSincronizacionMateriaPrima(0, 0, 0, 0, 1);
        }

        var codigosSap = materiasPrimaSap.Select(m => m.ItemCode).ToHashSet();

        var existentes = await _materiaPrimaRepository.ObtenerAsync();
        var existentesPorCodigo = existentes.ToDictionary(m => m.CodigoSap, m => m);

        foreach (var materiaPrimaSap in materiasPrimaSap)
        {
            try
            {
                if (!existentesPorCodigo.TryGetValue(materiaPrimaSap.ItemCode, out var existente))
                {
                    var entidad = new MateriaPrima
                    {
                        CodigoSap = materiaPrimaSap.ItemCode,
                        DescripcionSap = materiaPrimaSap.ItemName,
                        Activo = materiaPrimaSap.Activo,
                    };
                    var id = await _materiaPrimaRepository.InsertarAsync(entidad);

                    var creado = (await _materiaPrimaRepository.ObtenerAsync(id)).FirstOrDefault();
                    await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);
                    nuevos++;
                    continue;
                }

                if (existente.DescripcionSap != materiaPrimaSap.ItemName)
                {
                    await _materiaPrimaRepository.ActualizarDatosSapAsync(existente.Id, materiaPrimaSap.ItemName);

                    var actualizado = (await _materiaPrimaRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
                    await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, actualizado);
                    actualizados++;
                }

                if (!existente.Activo && materiaPrimaSap.Activo)
                {
                    await _materiaPrimaRepository.ActualizarActivoAsync(existente.Id, true);

                    var reactivado = (await _materiaPrimaRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
                    await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, reactivado);
                    reactivados++;
                }
            }
            catch (SqlRepositoryException)
            {
                errores++;
            }
        }

        foreach (var existente in existentes)
        {
            if (!existente.Activo || codigosSap.Contains(existente.CodigoSap))
            {
                continue;
            }

            try
            {
                await _materiaPrimaRepository.ActualizarActivoAsync(existente.Id, false);

                var desactivado = (await _materiaPrimaRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
                await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, desactivado);
                desactivados++;
            }
            catch (SqlRepositoryException)
            {
                errores++;
            }
        }

        return new ResultadoSincronizacionMateriaPrima(nuevos, actualizados, reactivados, desactivados, errores);
    }

    // Las materias primas solo se crean vía sincronización con SAP (SincronizarConSapAsync);
    // este método solo actualiza los campos de negocio capturados por el usuario en el form de edición.
    public async Task ActualizarAsync(MateriaPrimaDto datos)
    {
        var resultado = _validator.Validate(datos);
        if (!resultado.IsValid)
        {
            throw new ValidationException(string.Join(" ", resultado.Errors.Select(e => e.ErrorMessage)));
        }

        var anterior = (await _materiaPrimaRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La materia prima que intentas actualizar ya no existe.");

        await _materiaPrimaRepository.ActualizarAsync(new MateriaPrima
        {
            Id = datos.Id,
            CategoriaId = datos.CategoriaId,
            CalibreApeamId = datos.CalibreApeamId,
        });

        var nuevo = (await _materiaPrimaRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, MateriaPrima? anterior, MateriaPrima? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static MateriaPrimaDto MapearDto(MateriaPrima m) => new(
        m.Id,
        m.CodigoSap,
        m.DescripcionSap,
        m.Activo,
        m.CategoriaId,
        m.CalibreApeamId,
        m.FechaCreacion,
        m.CategoriaNombre,
        m.CalibreApeamNombre);
}
