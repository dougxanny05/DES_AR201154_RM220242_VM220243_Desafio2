using Eventos.Entities.DTO;

namespace Eventos.BL.Interfaces
{
    public interface IParticipanteService
    {
        public Task<List<ParticipanteDto>> GetParticipantesAsync();
        public Task<ParticipanteDto?> GetParticipanteByIdAsync(int id);
        public Task<ParticipanteDto> InsertParticipanteAsync(ParticipanteDto participante);
        public Task<ParticipanteDto?> UpdateParticipanteAsync(int id, ParticipanteDto participante);
        public Task<bool> DeleteParticipanteAsync(int id);
    }
}
