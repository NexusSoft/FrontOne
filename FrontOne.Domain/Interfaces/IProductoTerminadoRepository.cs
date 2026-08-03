using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IProductoTerminadoRepository
{
    Task<IReadOnlyList<ProductoTerminado>> ObtenerAsync(int? id = null);

    // Carga inicial del listado (sin filtro) para que el grid no se vea vacío al abrir.
    Task<IReadOnlyList<ProductoTerminado>> ObtenerTop100Async();

    // Búsqueda por texto (CodigoSap/DescripcionSap), TOP 500.
    Task<IReadOnlyList<ProductoTerminado>> BuscarAsync(string filtro);

    // Alta mínima usada únicamente por la sincronización con SAP.
    Task<int> InsertarAsync(ProductoTerminado producto);

    // Actualiza solo los campos espejo de SAP (Descripcion/DescripcionExtranjera), usado por la sincronización.
    Task ActualizarDatosSapAsync(int id, string descripcionSap, string? descripcionExtranjeraSap);

    // Activa/desactiva el registro (espejo de Valid en SAP), usado por la sincronización.
    Task ActualizarActivoAsync(int id, bool activo);

    // Actualización completa de los campos de negocio, capturados por el usuario en el form de edición.
    Task ActualizarAsync(ProductoTerminado producto);
}
