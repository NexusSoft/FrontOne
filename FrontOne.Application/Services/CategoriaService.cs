using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class CategoriaService
{
    private const string Modulo = "Catalogos";

    private readonly ICategoriaRepository _categoriaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CategoriaService(ICategoriaRepository categoriaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _categoriaRepository = categoriaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<CategoriaDto>> ObtenerAsync()
    {
        var categorias = await _categoriaRepository.ObtenerAsync();
        return categorias.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _categoriaRepository.InsertarAsync(new Categoria
        {
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _categoriaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _categoriaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _categoriaRepository.ActualizarAsync(new Categoria
        {
            Id = id,
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _categoriaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _categoriaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _categoriaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Categoria? anterior, Categoria? nuevo)
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
            throw new ValidationException("El nombre de la categoría es obligatorio");
        }
    }

    private static CategoriaDto MapearDto(Categoria categoria) => new(categoria.Id, categoria.Nombre, categoria.Activo);
}
