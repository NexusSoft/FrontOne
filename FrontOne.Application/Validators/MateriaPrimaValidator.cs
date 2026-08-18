using FluentValidation;
using FrontOne.Domain.DTOs;

namespace FrontOne.Application.Validators;

// Solo se invoca desde MateriaPrimaService.ActualizarAsync, cuando el usuario captura
// Categoría/Calibre APEAM y da clic en Guardar. La sincronización con SAP
// (SincronizarConSapAsync) NUNCA pasa por este validador, porque una materia prima recién
// sincronizada todavía no tiene ninguno de estos campos capturados (por eso son nullable en
// la entidad/DTO).
public class MateriaPrimaValidator : AbstractValidator<MateriaPrimaDto>
{
    public MateriaPrimaValidator()
    {
        RuleFor(m => m.CategoriaId).GreaterThan(0).WithMessage("Selecciona la Categoría.");
        RuleFor(m => m.CalibreApeamId).GreaterThan(0).WithMessage("Selecciona el Calibre APEAM.");
    }
}
