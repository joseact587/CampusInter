using System.ComponentModel.DataAnnotations;

namespace CampusInter.Application.DTOs.Inscripciones;

public sealed class CrearInscripcionRequest
{
    // Atributos
    [Required(ErrorMessage = "Debe seleccionar las materias de la inscripcion.")]
    [MinLength(3, ErrorMessage = "Debe seleccionar exactamente 3 materias.")]
    [MaxLength(3, ErrorMessage = "Debe seleccionar exactamente 3 materias.")]
    public IReadOnlyCollection<int> MateriasIds { get; init; } = [];
}
