using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ProductoService
{
    private const string Modulo = "Catalogos";

    private readonly IProductoRepository _productoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ProductoService(IProductoRepository productoRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _productoRepository = productoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<ProductoDto>> ObtenerAsync()
    {
        var productos = await _productoRepository.ObtenerAsync();
        return productos.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _productoRepository.InsertarAsync(new Producto { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _productoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _productoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _productoRepository.ActualizarAsync(new Producto { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _productoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _productoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _productoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Producto? anterior, Producto? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del producto es obligatorio");
        }
    }

    private static ProductoDto MapearDto(Producto producto) => new(producto.Id, producto.Nombre, producto.Activo);
}
