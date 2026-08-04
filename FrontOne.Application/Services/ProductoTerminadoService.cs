using System.Text.Json;
using FrontOne.Application.Validators;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public record ResultadoSincronizacionProductoTerminado(int Nuevos, int Actualizados, int Reactivados, int Desactivados, int Errores);

public class ProductoTerminadoService
{
    private const string Modulo = "Catalogos";

    // Nombre del grupo de artículos "Producto Terminado" en SAP Business One (ItemGroups.GroupName).
    // Si el nombre del grupo cambia en el ambiente de producción, hay que actualizar esta constante.
    private const string GrupoProductoTerminadoNombre = "PT";

    // Grupo de artículos "Materia Prima" en SAP — usado solo para llenar el LookUpEdit de
    // Materia Prima en ProductoTerminadoEditarForm, no se sincroniza ni se persiste catálogo local.
    private const string GrupoMateriaPrimaNombre = "MP";

    private readonly IProductoTerminadoRepository _productoTerminadoRepository;
    private readonly ISapItemRepository _sapItemRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ProductoTerminadoValidator _validator;

    public ProductoTerminadoService(
        IProductoTerminadoRepository productoTerminadoRepository,
        ISapItemRepository sapItemRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider,
        ProductoTerminadoValidator validator)
    {
        _productoTerminadoRepository = productoTerminadoRepository;
        _sapItemRepository = sapItemRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
        _validator = validator;
    }

    public async Task<IReadOnlyList<ProductoTerminadoDto>> ObtenerAsync()
    {
        var productos = await _productoTerminadoRepository.ObtenerAsync();
        return productos.Select(MapearDto).ToList();
    }

    public async Task<ProductoTerminadoDto?> ObtenerPorIdAsync(int id)
    {
        var productos = await _productoTerminadoRepository.ObtenerAsync(id);
        return productos.Select(MapearDto).FirstOrDefault();
    }

    // Carga inicial del listado (sin filtro) para que el grid no se vea vacío al abrir.
    public async Task<IReadOnlyList<ProductoTerminadoDto>> ObtenerTop1000Async()
    {
        var productos = await _productoTerminadoRepository.ObtenerTop1000Async();
        return productos.Select(MapearDto).ToList();
    }

    // Búsqueda para el listado: filtro server-side, máximo 500 resultados.
    public async Task<IReadOnlyList<ProductoTerminadoDto>> BuscarAsync(string filtro)
    {
        var productos = await _productoTerminadoRepository.BuscarAsync(filtro);
        return productos.Select(MapearDto).ToList();
    }

    // Trae de SAP los productos terminados vigentes (grupo "PT") y sincroniza contra la tabla
    // local: agrega los que faltan (con campos de negocio pendientes de captura), actualiza la
    // descripción si cambió en SAP, reactiva/desactiva por Valid, y nunca borra ningún registro
    // (un producto desactivado conserva toda su captura de negocio por si SAP lo reactiva después).
    public async Task<ResultadoSincronizacionProductoTerminado> SincronizarConSapAsync(CancellationToken cancellationToken = default)
    {
        var nuevos = 0;
        var actualizados = 0;
        var reactivados = 0;
        var desactivados = 0;
        var errores = 0;

        IReadOnlyList<SapProductoTerminadoDto> productosSap;
        try
        {
            productosSap = await _sapItemRepository.ObtenerPorGrupoAsync(GrupoProductoTerminadoNombre, cancellationToken);
        }
        catch (SapException)
        {
            // No se pudo consultar SAP (grupo no encontrado, Service Layer caído, etc.):
            // se reporta como un error de sincronización sin tumbar el listado local.
            return new ResultadoSincronizacionProductoTerminado(0, 0, 0, 0, 1);
        }

        var codigosSap = productosSap.Select(p => p.ItemCode).ToHashSet();

        var existentes = await _productoTerminadoRepository.ObtenerAsync();
        var existentesPorCodigo = existentes.ToDictionary(p => p.CodigoSap, p => p);

        foreach (var productoSap in productosSap)
        {
            try
            {
                if (!existentesPorCodigo.TryGetValue(productoSap.ItemCode, out var existente))
                {
                    var entidad = new ProductoTerminado
                    {
                        CodigoSap = productoSap.ItemCode,
                        DescripcionSap = productoSap.ItemName,
                        DescripcionExtranjeraSap = productoSap.ForeignName,
                        Activo = productoSap.Activo,
                    };
                    var id = await _productoTerminadoRepository.InsertarAsync(entidad);

                    var creado = (await _productoTerminadoRepository.ObtenerAsync(id)).FirstOrDefault();
                    await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);
                    nuevos++;
                    continue;
                }

                if (existente.DescripcionSap != productoSap.ItemName || existente.DescripcionExtranjeraSap != productoSap.ForeignName)
                {
                    await _productoTerminadoRepository.ActualizarDatosSapAsync(existente.Id, productoSap.ItemName, productoSap.ForeignName);

                    var actualizado = (await _productoTerminadoRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
                    await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, actualizado);
                    actualizados++;
                }

                if (!existente.Activo && productoSap.Activo)
                {
                    await _productoTerminadoRepository.ActualizarActivoAsync(existente.Id, true);

                    var reactivado = (await _productoTerminadoRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
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
                await _productoTerminadoRepository.ActualizarActivoAsync(existente.Id, false);

                var desactivado = (await _productoTerminadoRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
                await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, desactivado);
                desactivados++;
            }
            catch (SqlRepositoryException)
            {
                errores++;
            }
        }

        return new ResultadoSincronizacionProductoTerminado(nuevos, actualizados, reactivados, desactivados, errores);
    }

    // Lista de materiales (carta técnica) del producto, consultada directo en SAP sin persistir nada.
    public Task<IReadOnlyList<SapListaMaterialesDto>> ObtenerListaMaterialesAsync(string codigoSap)
        => _sapItemRepository.ObtenerListaMaterialesAsync(codigoSap);

    // Artículos SAP del grupo "MP" para llenar el LookUpEdit de Materia Prima — en vivo, no se
    // guarda catálogo local (mismo criterio que la Carta de materiales).
    public Task<IReadOnlyList<SapProductoTerminadoDto>> ObtenerMateriasPrimaAsync(CancellationToken cancellationToken = default)
        => _sapItemRepository.ObtenerPorGrupoAsync(GrupoMateriaPrimaNombre, cancellationToken);

    // Los productos terminados solo se crean vía sincronización con SAP (SincronizarConSapAsync);
    // este método solo actualiza los campos de negocio capturados por el usuario en el form de edición.
    public async Task ActualizarAsync(ProductoTerminadoDto datos)
    {
        var resultado = _validator.Validate(datos);
        if (!resultado.IsValid)
        {
            throw new ValidationException(string.Join(" ", resultado.Errors.Select(e => e.ErrorMessage)));
        }

        var anterior = (await _productoTerminadoRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("El producto terminado que intentas actualizar ya no existe.");

        await _productoTerminadoRepository.ActualizarAsync(new ProductoTerminado
        {
            Id = datos.Id,
            CodigoUpc = datos.CodigoUpc,
            CodigoPlu = datos.CodigoPlu,
            CodigoGtin = datos.CodigoGtin,
            MateriaPrimaItemCode = datos.MateriaPrimaItemCode,
            CategoriaId = datos.CategoriaId,
            TipoProductoId = datos.TipoProductoId,
            CalibreApeamId = datos.CalibreApeamId,
            CalibreCodigoExterno = datos.CalibreCodigoExterno,
            MercadoDestinoPaisId = datos.MercadoDestinoPaisId,
            MarcaId = datos.MarcaId,
            VariedadId = datos.VariedadId,
            PesoEstandarId = datos.PesoEstandarId,
            PesoNeto = datos.PesoNeto,
            PesoPromedio = datos.PesoPromedio,
            CajasPorPallet = datos.CajasPorPallet,
        });

        var nuevo = (await _productoTerminadoRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, ProductoTerminado? anterior, ProductoTerminado? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static ProductoTerminadoDto MapearDto(ProductoTerminado p) => new(
        p.Id,
        p.CodigoSap,
        p.DescripcionSap,
        p.DescripcionExtranjeraSap,
        p.Activo,
        p.CodigoUpc,
        p.CodigoPlu,
        p.CodigoGtin,
        p.MateriaPrimaItemCode,
        p.CategoriaId,
        p.TipoProductoId,
        p.CalibreApeamId,
        p.CalibreCodigoExterno,
        p.MercadoDestinoPaisId,
        p.MarcaId,
        p.VariedadId,
        p.PesoEstandarId,
        p.PesoNeto,
        p.PesoPromedio,
        p.CajasPorPallet,
        p.FechaCreacion);
}
