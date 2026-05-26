using AutoMapper;
using CampusInter.Application.DTOs.Inscripciones;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Mappings.Inscripciones;

public sealed class ConsultarCompanerosInscripcionProfile : Profile
{
    // Constructores
    public ConsultarCompanerosInscripcionProfile()
    {
        CreateMap<Estudiante, CompaneroResponse>()
            .ForMember(
                destino => destino.NombreCompleto,
                opciones => opciones.MapFrom(origen =>
                    string.Join(" ", new[]
                    {
                        origen.PrimerNombre,
                        origen.SegundoNombre,
                        origen.PrimerApellido,
                        origen.SegundoApellido
                    }.Where(valor => !string.IsNullOrWhiteSpace(valor)))));
    }
}
