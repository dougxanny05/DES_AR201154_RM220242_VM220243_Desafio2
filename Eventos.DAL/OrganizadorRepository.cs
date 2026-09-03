using Eventos.DAL.Interfaces;
using Eventos.Entities.Models;
using System.Linq;

namespace Eventos.DAL
{
    public class OrganizadorRepository(IDatabaseRepository databaseRepository) : IOrganizadorRepository
    {
        private static class Queries
        {
            public const string GetAll = "SELECT * FROM Organizadores";
            public const string GetById = "SELECT * FROM Organizadores WHERE Id = @Id";
            public const string Insert = "INSERT INTO Organizadores (Nombre, Cargo, EventoId) VALUES (@Nombre, @Cargo, @EventoId); SELECT SCOPE_IDENTITY()";
            public const string Update = "UPDATE Organizadores SET Nombre = @Nombre, Cargo = @Cargo, EventoId = @EventoId WHERE Id = @Id";
            public const string Delete = "DELETE FROM Organizadores WHERE Id = @Id";
        }

        public async Task<List<Organizador>> GetOrganizadoresAsync()
        {
            var items = await databaseRepository.QueryAsync<Organizador>(Queries.GetAll);
            return items.ToList();
        }

        public async Task<Organizador?> GetOrganizadorByIdAsync(int id)
        {
            return await databaseRepository.QueryFirstOrDefaultAsync<Organizador>(Queries.GetById, new { Id = id });
        }

        public async Task<int> InsertOrganizadorAsync(Organizador organizador)
        {
            return await databaseRepository.ExecuteScalarAsync<int>(Queries.Insert, new { organizador.Nombre, organizador.Cargo, organizador.EventoId });
        }

        public async Task<bool> UpdateOrganizadorAsync(Organizador organizador)
        {
            var rowsAffected = await databaseRepository.ExecuteAsync(Queries.Update, new { organizador.Id, organizador.Nombre, organizador.Cargo, organizador.EventoId });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteOrganizadorAsync(int id)
        {
            return await databaseRepository.ExecuteAsync(Queries.Delete, new { Id = id }) > 0;
        }
    }
}
