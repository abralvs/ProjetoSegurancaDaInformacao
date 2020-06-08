using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Models;
using SeguradosAPI.Services;

namespace SeguradosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]

    public class TematicaController : ControllerBase
    {
        private readonly ITematicaService _service;
        public TematicaController(ITematicaService service)
        {
            _service = service;
        }
        // GET: api/Tematica
        [HttpGet]
        public IActionResult Get()
        {
            var tematica = _service.ObterTodos();
            if (tematica.Count != 0)
                return Ok(tematica);

            return NoContent();
        }

        // GET: api/Tematica/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tematica = _service.ObterPorId(id);
            if (tematica != null)
                return Ok(tematica);

            return NoContent();
        }

        
        // POST: api/Tematica
        [HttpPost]
        public IActionResult Post([FromBody] Models.TematicaModel tematica)
        {
            if (_service.Add(tematica))
                return Ok(tematica);

            return null;
        }

        // PUT: api/Tematica/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Models.TematicaModel tematica)
        {
            if (_service.Atualizar(tematica))
                return Ok(tematica);

            return null;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => _service.Remover(id) ? Ok() : null;
    }
}
