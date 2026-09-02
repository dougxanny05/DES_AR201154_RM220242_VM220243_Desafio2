using Eventos.Entities.Models;

namespace Eventos.DAL.Interfaces
{
    public interface IOrganizadorRepository
    {
        public Task<List<Organizador>> GetOrganizadoresAsync();
        public Task<Organizador?> GetOrganizadorByIdAsync(int id);
        public Task<int> InsertOrganizadorAsync(Organizador organizador);
        public Task<bool> UpdateOrganizadorAsync(Organizador organizador);
        public Task<bool> DeleteOrganizadorAsync(int id);
    }
}
