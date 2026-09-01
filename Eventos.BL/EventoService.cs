using AutoMapper;
using Eventos.BL.Interfaces;
using Eventos.DAL.Interfaces;
using Eventos.Entities.DTO;
using Eventos.Entities.Models;

namespace Eventos.BL
{
    public class EventoService(IEventoRepository eventoRepository, IMapper mapper) : IEventoService
    {
        public async Task<List<EventoDto>> GetEventosAsync()
        {
            var eventos = await eventoRepository.GetEventosAsync();
            return mapper.Map<List<EventoDto>>(eventos);
        }

        public async Task<EventoDto?> GetEventoByIdAsync(int id)
        {
            var evento = await eventoRepository.GetEventoByIdAsync(id);
            return mapper.Map<EventoDto?>(evento);
        }

        public async Task<EventoDto> InsertEventoAsync(EventoDto evento)
        {
            var entity = mapper.Map<Evento>(evento);
            var newId = await eventoRepository.InsertEventoAsync(entity);
            evento.Codigo = newId;
            return evento;
        }

        public async Task<EventoDto?> UpdateEventoAsync(int id, EventoDto evento)
        {
            var entity = mapper.Map<Evento>(evento);
            entity.Id = id;
            var updated = await eventoRepository.UpdateEventoAsync(entity);
            if (!updated)
            {
                return null;
            }
            evento.Codigo = id;
            return evento;
        }

        public async Task<bool> DeleteEventoAsync(int id)
        {
            return await eventoRepository.DeleteEventoAsync(id);
        }
    }
}
