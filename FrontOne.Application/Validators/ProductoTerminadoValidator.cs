using FluentValidation;
using FrontOne.Domain.DTOs;

namespace FrontOne.Application.Validators;

// Primer uso de FluentValidation en el proyecto: solo se invoca desde ProductoTerminadoService.ActualizarAsync,
// cuando el usuario captura información de negocio y da clic en Guardar. La sincronización con SAP
// (SincronizarConSapAsync) NUNCA pasa por este validador, porque un producto recién sincronizado
// todavía no tiene ninguno de estos campos capturados (por eso son nullable en la entidad/DTO).
public class ProductoTerminadoValidator : AbstractValidator<ProductoTerminadoDto>
{
    public ProductoTerminadoValidator()
    {
        RuleFor(p => p.CategoriaId).GreaterThan(0).WithMessage("Selecciona la Categoría.");
        RuleFor(p => p.TipoProductoId).GreaterThan(0).WithMessage("Selecciona el Tipo de Producto.");
        RuleFor(p => p.CalibreApeamId).GreaterThan(0).WithMessage("Selecciona el Calibre APEAM.");
        RuleFor(p => p.MercadoDestinoPaisId).GreaterThan(0).WithMessage("Selecciona el Mercado Destino.");
        RuleFor(p => p.MarcaId).GreaterThan(0).WithMessage("Selecciona la Marca.");
        RuleFor(p => p.VariedadId).GreaterThan(0).WithMessage("Selecciona la Variedad.");
        RuleFor(p => p.PesoEstandarId).GreaterThan(0).WithMessage("Selecciona el Peso Estándar.");
        RuleFor(p => p.PesoPromedio).GreaterThan(0).WithMessage("El Peso Promedio debe ser mayor a cero.");
        RuleFor(p => p.CajasPorPallet).GreaterThan(0).WithMessage("Las Cajas por Pallet deben ser mayor a cero.");
        // CodigoUpc/CodigoPlu/CodigoGtin son opcionales, sin regla.
    }
}
