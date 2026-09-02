using Eventos.Entities.Models;

namespace Eventos.DAL.Interfaces
{
    public interface IEventoRepository
    {
        public Task<List<Evento>> GetEventosAsync();
        public Task<Evento?> GetEventoByIdAsync(int id);
        public Task<int> InsertEventoAsync(Evento evento);
        public Task<bool> UpdateEventoAsync(Evento evento);
        public Task<bool> DeleteEventoAsync(int id);
    }
}
