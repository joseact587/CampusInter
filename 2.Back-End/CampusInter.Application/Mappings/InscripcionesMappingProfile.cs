using AutoMapper;
using CampusInter.Application.DTOs.Inscripciones;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Mappings;

public sealed class InscripcionesMappingProfile : Profile
{
    // Constructores
    public InscripcionesMappingProfile()
    {
        CreateMap<InscripcionMateria, InscripcionMateriaResponse>()
            .ForMember(
                destino => destino.Nombre,
                opciones => opciones.MapFrom(origen => origen.Materia.Nombre))
            .ForMember(
                destino => destino.Creditos,
                opciones => opciones.MapFrom(origen => origen.Materia.Creditos))
            .ForMember(
                destino => destino.ProfesorId,
                opciones => opciones.MapFrom(origen => origen.Materia.ProfesorId))
            .ForMember(
                destino => destino.ProfesorNombre,
                opciones => opciones.MapFrom(origen => ArmarNombreProfesor(
                    origen.Materia.Profesor.PrimerNombre,
                    origen.Materia.Profesor.SegundoNombre,
                    origen.Materia.Profesor.PrimerApellido,
                    origen.Materia.Profesor.SegundoApellido)));

        CreateMap<Inscripcion, InscripcionResponse>()
            .ForMember(
                destino => destino.Estado,
                opciones => opciones.MapFrom(origen => origen.Estado.ToString()))
            .ForMember(
                destino => destino.Materias,
                opciones => opciones.MapFrom(origen => origen.InscripcionesMateria));
    }

    // Utilidades
    private static string ArmarNombreProfesor(
        string primerNombre,
        string? segundoNombre,
        string primerApellido,
        string? segundoApellido)
    {
        return string.Join(" ", new[]
        {
            primerNombre,
            segundoNombre,
            primerApellido,
            segundoApellido
        }.Where(valor => !string.IsNullOrWhiteSpace(valor)));
    }
}
