using AutoMapper;
using Eventos.Entities.DTO;
using Eventos.Entities.Models;

namespace Eventos.BL.Profiles
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Evento, EventoDto>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreEvento, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.FechaEvento, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.LugarEvento, opt => opt.MapFrom(src => src.Lugar))
                .ReverseMap();
        }
    }
}
