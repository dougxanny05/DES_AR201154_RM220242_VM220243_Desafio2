using Eventos.Entities.DTO;

namespace Eventos.BL.Interfaces
{
    public interface IEventoService
    {
        public Task<List<EventoDto>> GetEventosAsync();
        public Task<EventoDto?> GetEventoByIdAsync(int id);
        public Task<EventoDto> InsertEventoAsync(EventoDto evento);
        public Task<EventoDto?> UpdateEventoAsync(int id, EventoDto evento);
        public Task<bool> DeleteEventoAsync(int id);
    }
}
