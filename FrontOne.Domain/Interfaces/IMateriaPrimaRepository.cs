using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IMateriaPrimaRepository
{
    Task<IReadOnlyList<MateriaPrima>> ObtenerAsync(int? id = null);

    // Carga inicial del listado (sin filtro) para que el grid no se vea vacío al abrir.
    Task<IReadOnlyList<MateriaPrima>> ObtenerTop1000Async();

    // Búsqueda por texto (CodigoSap/DescripcionSap), TOP 500.
    Task<IReadOnlyList<MateriaPrima>> BuscarAsync(string filtro);

    // Alta mínima usada únicamente por la sincronización con SAP.
    Task<int> InsertarAsync(MateriaPrima materiaPrima);

    // Actualiza solo el campo espejo de SAP (Descripcion), usado por la sincronización.
    Task ActualizarDatosSapAsync(int id, string descripcionSap);

    // Activa/desactiva el registro (espejo de Valid en SAP), usado por la sincronización.
    Task ActualizarActivoAsync(int id, bool activo);

    // Actualización completa de los campos de negocio, capturados por el usuario en el form de edición.
    Task ActualizarAsync(MateriaPrima materiaPrima);
}
