using AutoMapper;
using CampusInter.Application.DTOs.Materias;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Mappings;

public sealed class MateriasMappingProfile : Profile
{
    // Constructores
    public MateriasMappingProfile()
    {
        CreateMap<Materia, MateriaResponse>()
            .ForMember(
                destino => destino.ProfesorNombre,
                opciones => opciones.MapFrom(origen => ArmarNombreProfesor(
                    origen.Profesor.PrimerNombre,
                    origen.Profesor.SegundoNombre,
                    origen.Profesor.PrimerApellido,
                    origen.Profesor.SegundoApellido)));
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
