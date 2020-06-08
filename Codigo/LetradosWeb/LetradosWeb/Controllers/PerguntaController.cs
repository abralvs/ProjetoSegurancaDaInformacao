using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Models;
using SeguradosAPI.Services;

namespace SeguradosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class PerguntaController : ControllerBase
    {
        private readonly IPerguntaService _service;
        public PerguntaController(IPerguntaService service)
        {
            _service = service;
        }
        // GET: api/Pergunta
        [HttpGet]
        public IActionResult Get()
        {
            var perguntas = _service.ObterTodos();
            if (perguntas.Count != 0)
                return Ok(perguntas);

            return NoContent();
        }

        // GET: api/Pergunta/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var pergunta = _service.ObterPorId(id);
            if (pergunta != null)
                return Ok(pergunta);

            return NoContent();
        }

        // POST: api/Pergunta
        [HttpPost]
        public IActionResult Post([FromBody] PerguntaModel pergunta)
        {
            if (_service.Add(pergunta))
                return Ok(pergunta);

            return null;
        }

        // PUT: api/Pergunta/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] PerguntaModel pergunta)
        {
            if (_service.Atualizar(pergunta))
                return Ok(pergunta);

            return null;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => _service.Remover(id) ? Ok() : null;
    }
}
