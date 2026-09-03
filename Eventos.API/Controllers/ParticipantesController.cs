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
    public class ParticipantesController : ControllerBase
    {
        private readonly IParticipanteRepository _repo;
        private readonly IDistributedCache _cache;

        public ParticipantesController(IParticipanteRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cacheKey = "participantes:all";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var cachedList = JsonSerializer.Deserialize<List<Participante>>(cached);
                return Ok(cachedList);
            }

            var items = await _repo.GetParticipantesAsync();
            var opt = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(items), opt);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var cacheKey = $"participantes:{id}";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var item = JsonSerializer.Deserialize<Participante>(cached);
                return Ok(item);
            }

            var participante = await _repo.GetParticipanteByIdAsync(id);
            if (participante == null) return NotFound();

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(participante), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
            return Ok(participante);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ParticipanteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Participante { Nombre = dto.NombreParticipante, Email = dto.Email, EventoId = dto.EventoId };
            var id = await _repo.InsertParticipanteAsync(model);
            await _cache.RemoveAsync("participantes:all");
            return CreatedAtAction(nameof(Get), new { id }, new { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ParticipanteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Participante { Id = id, Nombre = dto.NombreParticipante, Email = dto.Email, EventoId = dto.EventoId };
            var ok = await _repo.UpdateParticipanteAsync(model);
            if (!ok) return NotFound();

            await _cache.RemoveAsync("participantes:all");
            await _cache.RemoveAsync($"participantes:{id}");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteParticipanteAsync(id);
            if (!ok) return NotFound();

            await _cache.RemoveAsync("participantes:all");
            await _cache.RemoveAsync($"participantes:{id}");
            return NoContent();
        }
    }
}
