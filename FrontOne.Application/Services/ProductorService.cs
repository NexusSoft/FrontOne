using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ProductorService
{
    private const string Modulo = "Catalogos";

    private readonly IProductorRepository _productorRepository;
    private readonly ICryptoService _cryptoService;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ProductorService(
        IProductorRepository productorRepository,
        ICryptoService cryptoService,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _productorRepository = productorRepository;
        _cryptoService = cryptoService;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<ProductorDto>> ObtenerAsync()
    {
        var productores = await _productorRepository.ObtenerAsync();
        return productores.Select(MapearDto).ToList();
    }

    public async Task<ProductorDto?> ObtenerPorIdAsync(int id)
    {
        var productores = await _productorRepository.ObtenerAsync(id);
        return productores.Select(MapearDto).FirstOrDefault();
    }

    // Búsqueda para el picker: filtro server-side, máximo 500 resultados.
    public async Task<IReadOnlyList<ProductorDto>> BuscarAsync(string filtro)
    {
        var productores = await _productorRepository.BuscarAsync(filtro);
        return productores.Select(MapearDto).ToList();
    }

    // Carga inicial del picker (sin filtro) para que el grid no se vea vacío al abrir.
    public async Task<IReadOnlyList<ProductorDto>> ObtenerTop100Async()
    {
        var productores = await _productorRepository.ObtenerTop100Async();
        return productores.Select(MapearDto).ToList();
    }

    // Navegación del catálogo completo por Id (orden de creación) — seek puntual,
    // nunca carga todos los productores a memoria. Usado por Inicio/Anterior/Siguiente/Fin.
    public async Task<ProductorDto?> ObtenerPrimeroAsync()
    {
        var productor = await _productorRepository.ObtenerPrimeroAsync();
        return productor is null ? null : MapearDto(productor);
    }

    public async Task<ProductorDto?> ObtenerUltimoAsync()
    {
        var productor = await _productorRepository.ObtenerUltimoAsync();
        return productor is null ? null : MapearDto(productor);
    }

    public async Task<ProductorDto?> ObtenerSiguienteAsync(int id)
    {
        var productor = await _productorRepository.ObtenerSiguienteAsync(id);
        return productor is null ? null : MapearDto(productor);
    }

    public async Task<ProductorDto?> ObtenerAnteriorAsync(int id)
    {
        var productor = await _productorRepository.ObtenerAnteriorAsync(id);
        return productor is null ? null : MapearDto(productor);
    }

    public async Task<(int Id, string Clave)> CrearAsync(ProductorDto datos)
    {
        ValidarCampos(datos);

        var resultado = await _productorRepository.InsertarAsync(MapearEntidad(datos));

        var creado = await _productorRepository.ObtenerAsync(resultado.Id);
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado.FirstOrDefault());

        return resultado;
    }

    public async Task ActualizarAsync(ProductorDto datos)
    {
        ValidarCampos(datos);

        var anterior = (await _productorRepository.ObtenerAsync(datos.Id)).FirstOrDefault();

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _productorRepository.ActualizarAsync(entidad);

        var nuevo = (await _productorRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _productorRepository.ObtenerAsync(id)).FirstOrDefault();

        await _productorRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Productor? anterior, Productor? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(ProductorDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.NombreProductor))
        {
            throw new ValidationException("El nombre del productor es obligatorio");
        }
    }

    private Productor MapearEntidad(ProductorDto datos) => new()
    {
        NombreProductor = datos.NombreProductor.Trim(),
        Domicilio = datos.Domicilio,
        Colonia = datos.Colonia,
        CodigoPostal = datos.CodigoPostal,
        PoblacionId = datos.PoblacionId,
        MunicipioId = datos.MunicipioId,
        EstadoId = datos.EstadoId,
        Rfc = datos.Rfc,
        Telefono = datos.Telefono,
        Celular = datos.Celular,
        Email = datos.Email,
        Organizacion = datos.Organizacion,
        Observaciones = datos.Observaciones,
        Usuario = datos.Usuario,
        PasswordEncriptado = string.IsNullOrEmpty(datos.Password) ? null : _cryptoService.Encrypt(datos.Password),
        DiasCredito = datos.DiasCredito,
        Activo = datos.Activo,
    };

    private ProductorDto MapearDto(Productor p) => new(
        p.Id,
        p.Clave,
        p.FechaRegistro,
        p.NombreProductor,
        p.Domicilio,
        p.Colonia,
        p.CodigoPostal,
        p.PoblacionId,
        p.MunicipioId,
        p.EstadoId,
        p.Rfc,
        p.Telefono,
        p.Celular,
        p.Email,
        p.Organizacion,
        p.Observaciones,
        p.Usuario,
        string.IsNullOrEmpty(p.PasswordEncriptado) ? null : _cryptoService.Decrypt(p.PasswordEncriptado),
        p.DiasCredito,
        p.Activo);
}
