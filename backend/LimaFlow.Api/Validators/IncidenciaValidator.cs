using FluentValidation;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Validators;

public class IncidenciaValidator : AbstractValidator<Incidencia>
{
    public IncidenciaValidator()
    {
        RuleFor(i => i.Descripcion)
            .NotEmpty().WithMessage("La descripción de la incidencia es obligatoria.")
            .MinimumLength(10).WithMessage("Cuéntanos un poco más. La descripción debe tener al menos 10 caracteres.")
            .MaximumLength(500).WithMessage("La descripción es demasiado larga (máximo 500 caracteres).");

        RuleFor(i => i.ViaId)
            .GreaterThan(0).WithMessage("Debes asociar la incidencia a una vía válida.");

        RuleFor(i => i.UsuarioId)
            .GreaterThan(0).WithMessage("El reporte debe incluir un ID de usuario válido.");
    }
}