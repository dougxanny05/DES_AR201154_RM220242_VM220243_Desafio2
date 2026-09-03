using Eventos.DAL.Interfaces;
using Eventos.Entities.Models;
using System.Linq;

namespace Eventos.DAL
{
    public class EventoRepository(IDatabaseRepository databaseRepository) : IEventoRepository
    {
        private static class Queries
        {
            public const string GetAll = "SELECT * FROM Eventos";
            public const string GetById = "SELECT * FROM Eventos WHERE Id = @Id";
            public const string Insert = "INSERT INTO Eventos (Nombre, Fecha, Lugar) VALUES (@Nombre, @Fecha, @Lugar); SELECT SCOPE_IDENTITY()";
            public const string Update = "UPDATE Eventos SET Nombre = @Nombre, Fecha = @Fecha, Lugar = @Lugar WHERE Id = @Id";
            public const string Delete = "DELETE FROM Eventos WHERE Id = @Id";
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            var items = await databaseRepository.QueryAsync<Evento>(Queries.GetAll);
            return items.ToList();
        }

        public async Task<Evento?> GetEventoByIdAsync(int id)
        {
            return await databaseRepository.QueryFirstOrDefaultAsync<Evento>(Queries.GetById, new { Id = id });
        }

        public async Task<int> InsertEventoAsync(Evento evento)
        {
            return await databaseRepository.ExecuteScalarAsync<int>(Queries.Insert, new { evento.Nombre, evento.Fecha, evento.Lugar });
        }

        public async Task<bool> UpdateEventoAsync(Evento evento)
        {
            var rowsAffected = await databaseRepository.ExecuteAsync(Queries.Update, new { evento.Id, evento.Nombre, evento.Fecha, evento.Lugar });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteEventoAsync(int id)
        {
            return await databaseRepository.ExecuteAsync(Queries.Delete, new { Id = id }) > 0;
        }
    }
}
