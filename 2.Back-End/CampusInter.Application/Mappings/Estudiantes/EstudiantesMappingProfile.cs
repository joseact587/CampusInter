using AutoMapper;
using CampusInter.Application.DTOs.Estudiantes;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Mappings.Estudiantes;

public sealed class EstudiantesMappingProfile : Profile
{
    // Constructores
    public EstudiantesMappingProfile()
    {
        CreateMap<Estudiante, EstudianteResumenResponse>()
            .ForMember(
                destino => destino.Correo,
                opciones => opciones.MapFrom(origen => origen.Usuario.Correo))
            .ForMember(
                destino => destino.Estado,
                opciones => opciones.MapFrom(origen => origen.Estado.ToString()));

        CreateMap<Estudiante, MiPerfilResponse>()
            .ForMember(
                destino => destino.Correo,
                opciones => opciones.MapFrom(origen => origen.Usuario.Correo))
            .ForMember(
                destino => destino.Estado,
                opciones => opciones.MapFrom(origen => origen.Estado.ToString()));
    }
}
