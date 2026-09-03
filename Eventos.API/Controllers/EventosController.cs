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
    public class EventosController : ControllerBase
    {
        private readonly IEventoRepository _repo;
        private readonly IDistributedCache _cache;

        public EventosController(IEventoRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cacheKey = "eventos:all";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var cachedList = JsonSerializer.Deserialize<List<Evento>>(cached);
                return Ok(cachedList);
            }

            var items = await _repo.GetEventosAsync();
            var opt = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(items), opt);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var cacheKey = $"eventos:{id}";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var item = JsonSerializer.Deserialize<Evento>(cached);
                return Ok(item);
            }

            var evento = await _repo.GetEventoByIdAsync(id);
            if (evento == null) return NotFound();

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(evento), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
            return Ok(evento);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Evento { Nombre = dto.NombreEvento, Fecha = dto.FechaEvento, Lugar = dto.LugarEvento };
            var id = await _repo.InsertEventoAsync(model);

            // Invalidate caches
            await _cache.RemoveAsync("eventos:all");

            return CreatedAtAction(nameof(Get), new { id }, new { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] EventoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Evento { Id = id, Nombre = dto.NombreEvento, Fecha = dto.FechaEvento, Lugar = dto.LugarEvento };
            var ok = await _repo.UpdateEventoAsync(model);
            if (!ok) return NotFound();

            await _cache.RemoveAsync("eventos:all");
            await _cache.RemoveAsync($"eventos:{id}");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteEventoAsync(id);
            if (!ok) return NotFound();

            await _cache.RemoveAsync("eventos:all");
            await _cache.RemoveAsync($"eventos:{id}");
            return NoContent();
        }
    }
}
