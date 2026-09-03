using Eventos.DAL.Interfaces;
using Eventos.Entities.Models;
using System.Linq;

namespace Eventos.DAL
{
    public class ParticipanteRepository(IDatabaseRepository databaseRepository) : IParticipanteRepository
    {
        private static class Queries
        {
            public const string GetAll = "SELECT * FROM Participantes";
            public const string GetById = "SELECT * FROM Participantes WHERE Id = @Id";
            public const string Insert = "INSERT INTO Participantes (Nombre, Email, EventoId) VALUES (@Nombre, @Email, @EventoId); SELECT SCOPE_IDENTITY()";
            public const string Update = "UPDATE Participantes SET Nombre = @Nombre, Email = @Email, EventoId = @EventoId WHERE Id = @Id";
            public const string Delete = "DELETE FROM Participantes WHERE Id = @Id";
        }

        public async Task<List<Participante>> GetParticipantesAsync()
        {
            var items = await databaseRepository.QueryAsync<Participante>(Queries.GetAll);
            return items.ToList();
        }

        public async Task<Participante?> GetParticipanteByIdAsync(int id)
        {
            return await databaseRepository.QueryFirstOrDefaultAsync<Participante>(Queries.GetById, new { Id = id });
        }

        public async Task<int> InsertParticipanteAsync(Participante participante)
        {
            return await databaseRepository.ExecuteScalarAsync<int>(Queries.Insert, new { participante.Nombre, participante.Email, participante.EventoId });
        }

        public async Task<bool> UpdateParticipanteAsync(Participante participante)
        {
            var rowsAffected = await databaseRepository.ExecuteAsync(Queries.Update, new { participante.Id, participante.Nombre, participante.Email, participante.EventoId });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteParticipanteAsync(int id)
        {
            return await databaseRepository.ExecuteAsync(Queries.Delete, new { Id = id }) > 0;
        }
    }
}
