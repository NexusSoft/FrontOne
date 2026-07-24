using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public record SincronizacionListaPrecioCorteResultado(int Nuevos, int Actualizados, int Reactivados, int Desactivados);

public class ListaPrecioCorteService
{
    private const string Modulo = "Acopio";

    // Grupo de proveedores "Cosecha" en SAP Business One (BusinessPartnerGroups.Code = 104,
    // verificado en el ambiente de pruebas FRONTERRA_PROTO) — las empresas/personas que hacen
    // el corte de fruta están dadas de alta en SAP con este grupo. Si el código de grupo
    // cambia en el ambiente de producción, hay que actualizar esta constante.
    private const int GrupoCosechaCodigo = 104;

    private readonly IListaPrecioCorteRepository _listaPrecioCorteRepository;
    private readonly ISapProveedorRepository _sapProveedorRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ListaPrecioCorteService(
        IListaPrecioCorteRepository listaPrecioCorteRepository,
        ISapProveedorRepository sapProveedorRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _listaPrecioCorteRepository = listaPrecioCorteRepository;
        _sapProveedorRepository = sapProveedorRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    // Solo los proveedores activos (habilitados en SAP) se muestran en el grid — un proveedor
    // deshabilitado en SAP queda marcado Activo = false por la sincronización, sin perder su
    // precio capturado, pero desaparece de esta lista hasta que se vuelva a habilitar.
    public async Task<IReadOnlyList<ListaPrecioCorteDto>> ObtenerAsync()
    {
        var lista = await _listaPrecioCorteRepository.ObtenerAsync();
        return lista.Where(l => l.Activo).Select(MapearDto).ToList();
    }

    // Trae de SAP las empresas de corte vigentes (grupo Cosecha, CardType Supplier, Valid) y
    // sincroniza contra la tabla local:
    // - agrega las que faltan (con precios en $0, listas para capturar)
    // - actualiza el nombre de las que ya cambiaron en SAP
    // - reactiva (Activo = true) las que estaban inactivas y ahora vuelven a aparecer en SAP
    // - desactiva (Activo = false) las que ya no aparecen en SAP (se deshabilitaron allá) —
    //   nunca borra el renglón ni toca sus precios capturados.
    public async Task<SincronizacionListaPrecioCorteResultado> SincronizarConSapAsync(CancellationToken cancellationToken = default)
    {
        var proveedoresSap = await _sapProveedorRepository.ObtenerPorGrupoAsync(GrupoCosechaCodigo, cancellationToken);
        var cardCodesSap = proveedoresSap.Select(p => p.CardCode).ToHashSet();

        var existentes = await _listaPrecioCorteRepository.ObtenerAsync();
        var existentesPorCardCode = existentes
            .Where(l => l.CardCode is not null)
            .ToDictionary(l => l.CardCode!, l => l);

        var nuevos = 0;
        var actualizados = 0;
        var reactivados = 0;
        var desactivados = 0;

        foreach (var proveedor in proveedoresSap)
        {
            if (!existentesPorCardCode.TryGetValue(proveedor.CardCode, out var existente))
            {
                var entidad = new ListaPrecioCorte
                {
                    CardCode = proveedor.CardCode,
                    CardName = proveedor.CardName,
                    PrecioKg = 0,
                    PrecioDia = 0,
                    CuadrillaApoyo = 0,
                    Activo = true,
                };
                var id = await _listaPrecioCorteRepository.InsertarAsync(entidad);

                var creado = (await _listaPrecioCorteRepository.ObtenerAsync(id)).FirstOrDefault();
                await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);
                nuevos++;
                continue;
            }

            if (existente.CardName != proveedor.CardName)
            {
                await _listaPrecioCorteRepository.ActualizarNombreAsync(existente.Id, proveedor.CardName);
                actualizados++;
            }

            if (!existente.Activo)
            {
                await _listaPrecioCorteRepository.ActualizarActivoAsync(existente.Id, true);

                var reactivado = (await _listaPrecioCorteRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
                await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, reactivado);
                reactivados++;
            }
        }

        foreach (var existente in existentes)
        {
            if (existente.CardCode is null || !existente.Activo || cardCodesSap.Contains(existente.CardCode))
            {
                continue;
            }

            await _listaPrecioCorteRepository.ActualizarActivoAsync(existente.Id, false);

            var desactivado = (await _listaPrecioCorteRepository.ObtenerAsync(existente.Id)).FirstOrDefault();
            await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, existente, desactivado);
            desactivados++;
        }

        return new SincronizacionListaPrecioCorteResultado(nuevos, actualizados, reactivados, desactivados);
    }

    public async Task ActualizarPreciosAsync(int id, decimal precioKg, decimal precioDia, decimal cuadrillaApoyo)
    {
        if (precioKg < 0 || precioDia < 0 || cuadrillaApoyo < 0)
        {
            throw new ValidationException("Los precios no pueden ser negativos");
        }

        var anterior = (await _listaPrecioCorteRepository.ObtenerAsync(id)).FirstOrDefault()
            ?? throw new ValidationException("El renglón que intentas actualizar ya no existe.");

        await _listaPrecioCorteRepository.ActualizarAsync(new ListaPrecioCorte
        {
            Id = id,
            CardCode = anterior.CardCode,
            CardName = anterior.CardName,
            PrecioKg = precioKg,
            PrecioDia = precioDia,
            CuadrillaApoyo = cuadrillaApoyo,
            Activo = anterior.Activo,
        });

        var nuevo = (await _listaPrecioCorteRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, ListaPrecioCorte? anterior, ListaPrecioCorte? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static ListaPrecioCorteDto MapearDto(ListaPrecioCorte l) => new(
        l.Id,
        l.CardCode,
        l.CardName,
        l.PrecioKg,
        l.PrecioDia,
        l.CuadrillaApoyo,
        l.Activo);
}
