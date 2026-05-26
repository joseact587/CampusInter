using AutoMapper;
using CampusInter.Application.DTOs.Auth;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Mappings;

public sealed class AuthMappingProfile : Profile
{
    // Constructores
    public AuthMappingProfile()
    {
        CreateMap<Estudiante, AuthResponse>()
            .ForMember(
                destino => destino.AccessToken,
                opciones => opciones.MapFrom((_, _, _, contexto) => contexto.Items["AccessToken"]))
            .ForMember(
                destino => destino.Estado,
                opciones => opciones.MapFrom(origen => origen.Estado.ToString()));
    }
}
