using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Models;
using SeguradosAPI.Services;

namespace SeguradosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class UsuarioHasPerguntaController : ControllerBase
    {
        private readonly IUsuarioHasPerguntaService _service;
        public UsuarioHasPerguntaController(IUsuarioHasPerguntaService service)
        {
            _service = service;
        }
        // GET: api/UsuarioHasPergunta
        [HttpGet]
        public IActionResult Get()
        {
            var usuarioPergunta = _service.ObterTodos();
            if (usuarioPergunta.Count != 0)
                return Ok(usuarioPergunta);

            return NoContent();
        }

        // GET: api/UsuarioHasPergunta/5
        [HttpGet("{idUsuario}")]
        public IActionResult Get(int idUsuario)
        {
            var usuarioPergunta = _service.ObterPorId(idUsuario);
            if (usuarioPergunta != null)
                return Ok(usuarioPergunta);

            return NoContent();
        }

        // POST: api/UsuarioHasPergunta
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioHasPerguntaModel usuarioPergunta)
        {
            if (_service.Add(usuarioPergunta))
                return Ok(usuarioPergunta);

            return BadRequest(usuarioPergunta);
        }

        // PUT: api/UsuarioHasPergunta/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] UsuarioHasPerguntaModel usuarioPergunta)
        {
            if (_service.Atualizar(usuarioPergunta))
                return Ok(usuarioPergunta);

            return null;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{idUsuario}/{idPergunta}")]
        public IActionResult Delete(int idUsuario, int idPergunta)
            => _service.Remover(idUsuario, idPergunta) ? Ok() : null;
    }
}
