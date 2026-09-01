using AutoMapper;
using Eventos.BL.Interfaces;
using Eventos.DAL.Interfaces;
using Eventos.Entities.DTO;
using Eventos.Entities.Models;

namespace Eventos.BL
{
    public class ParticipanteService(IParticipanteRepository participanteRepository, IMapper mapper) : IParticipanteService
    {
        public async Task<List<ParticipanteDto>> GetParticipantesAsync()
        {
            var participantes = await participanteRepository.GetParticipantesAsync();
            return mapper.Map<List<ParticipanteDto>>(participantes);
        }

        public async Task<ParticipanteDto?> GetParticipanteByIdAsync(int id)
        {
            var participante = await participanteRepository.GetParticipanteByIdAsync(id);
            return mapper.Map<ParticipanteDto?>(participante);
        }

        public async Task<ParticipanteDto> InsertParticipanteAsync(ParticipanteDto participante)
        {
            var entity = mapper.Map<Participante>(participante);
            var newId = await participanteRepository.InsertParticipanteAsync(entity);
            participante.Codigo = newId;
            return participante;
        }

        public async Task<ParticipanteDto?> UpdateParticipanteAsync(int id, ParticipanteDto participante)
        {
            var entity = mapper.Map<Participante>(participante);
            entity.Id = id;
            var updated = await participanteRepository.UpdateParticipanteAsync(entity);
            if (!updated)
            {
                return null;
            }
            participante.Codigo = id;
            return participante;
        }

        public async Task<bool> DeleteParticipanteAsync(int id)
        {
            return await participanteRepository.DeleteParticipanteAsync(id);
        }
    }
}
