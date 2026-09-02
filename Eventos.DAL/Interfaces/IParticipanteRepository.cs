using Eventos.Entities.Models;

namespace Eventos.DAL.Interfaces
{
    public interface IParticipanteRepository
    {
        public Task<List<Participante>> GetParticipantesAsync();
        public Task<Participante?> GetParticipanteByIdAsync(int id);
        public Task<int> InsertParticipanteAsync(Participante participante);
        public Task<bool> UpdateParticipanteAsync(Participante participante);
        public Task<bool> DeleteParticipanteAsync(int id);
    }
}
