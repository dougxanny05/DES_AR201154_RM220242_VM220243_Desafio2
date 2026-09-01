using Eventos.Entities.DTO;

namespace Eventos.BL.Interfaces
{
    public interface IOrganizadorService
    {
        public Task<List<OrganizadorDto>> GetOrganizadoresAsync();
        public Task<OrganizadorDto?> GetOrganizadorByIdAsync(int id);
        public Task<OrganizadorDto> InsertOrganizadorAsync(OrganizadorDto organizador);
        public Task<OrganizadorDto?> UpdateOrganizadorAsync(int id, OrganizadorDto organizador);
        public Task<bool> DeleteOrganizadorAsync(int id);
    }
}
