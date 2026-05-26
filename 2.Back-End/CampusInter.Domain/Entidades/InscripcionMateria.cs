using CampusInter.Domain.Common.Auditing;

namespace CampusInter.Domain.Entidades;

public class InscripcionMateria : CreatedEntity
{
    // Atributos
    public int InscripcionId { get; private set; }
    public Inscripcion Inscripcion { get; private set; } = null!;
    public int MateriaId { get; private set; }
    public Materia Materia { get; private set; } = null!;

    // Constructores
    private InscripcionMateria()
    {
    }

    public InscripcionMateria(Inscripcion inscripcion, Materia materia)
    {
        ArgumentNullException.ThrowIfNull(inscripcion);
        ArgumentNullException.ThrowIfNull(materia);

        Inscripcion = inscripcion;
        Materia = materia;
        MateriaId = materia.MateriaId;
    }

}
