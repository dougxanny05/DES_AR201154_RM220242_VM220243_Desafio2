using AutoMapper;
using Eventos.BL.Interfaces;
using Eventos.DAL.Interfaces;
using Eventos.Entities.DTO;
using Eventos.Entities.Models;

namespace Eventos.BL
{
    public class OrganizadorService(IOrganizadorRepository organizadorRepository, IMapper mapper) : IOrganizadorService
    {
        public async Task<List<OrganizadorDto>> GetOrganizadoresAsync()
        {
            var organizadores = await organizadorRepository.GetOrganizadoresAsync();
            return mapper.Map<List<OrganizadorDto>>(organizadores);
        }

        public async Task<OrganizadorDto?> GetOrganizadorByIdAsync(int id)
        {
            var organizador = await organizadorRepository.GetOrganizadorByIdAsync(id);
            return mapper.Map<OrganizadorDto?>(organizador);
        }

        public async Task<OrganizadorDto> InsertOrganizadorAsync(OrganizadorDto organizador)
        {
            var entity = mapper.Map<Organizador>(organizador);
            var newId = await organizadorRepository.InsertOrganizadorAsync(entity);
            organizador.Codigo = newId;
            return organizador;
        }

        public async Task<OrganizadorDto?> UpdateOrganizadorAsync(int id, OrganizadorDto organizador)
        {
            var entity = mapper.Map<Organizador>(organizador);
            entity.Id = id;
            var updated = await organizadorRepository.UpdateOrganizadorAsync(entity);
            if (!updated)
            {
                return null;
            }
            organizador.Codigo = id;
            return organizador;
        }

        public async Task<bool> DeleteOrganizadorAsync(int id)
        {
            return await organizadorRepository.DeleteOrganizadorAsync(id);
        }
    }
}
