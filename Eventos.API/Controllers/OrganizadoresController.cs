using Eventos.DAL.Interfaces;
using Eventos.Entities.DTO;
using Eventos.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Eventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizadoresController : ControllerBase
    {
        private readonly IOrganizadorRepository _repo;
        private readonly IDistributedCache _cache;

        public OrganizadoresController(IOrganizadorRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cacheKey = "organizadores:all";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var cachedList = JsonSerializer.Deserialize<List<Organizador>>(cached);
                return Ok(cachedList);
            }

            var items = await _repo.GetOrganizadoresAsync();
            var opt = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(items), opt);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var cacheKey = $"organizadores:{id}";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var item = JsonSerializer.Deserialize<Organizador>(cached);
                return Ok(item);
            }

            var organizador = await _repo.GetOrganizadorByIdAsync(id);
            if (organizador == null) return NotFound();

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(organizador), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
            return Ok(organizador);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizadorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Organizador { Nombre = dto.NombreOrganizador, Cargo = dto.Cargo, EventoId = dto.EventoId };
            var id = await _repo.InsertOrganizadorAsync(model);
            await _cache.RemoveAsync("organizadores:all");
            return CreatedAtAction(nameof(Get), new { id }, new { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrganizadorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Organizador { Id = id, Nombre = dto.NombreOrganizador, Cargo = dto.Cargo, EventoId = dto.EventoId };
            var ok = await _repo.UpdateOrganizadorAsync(model);
            if (!ok) return NotFound();

            await _cache.RemoveAsync("organizadores:all");
            await _cache.RemoveAsync($"organizadores:{id}");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteOrganizadorAsync(id);
            if (!ok) return NotFound();

            await _cache.RemoveAsync("organizadores:all");
            await _cache.RemoveAsync($"organizadores:{id}");
            return NoContent();
        }
    }
}
