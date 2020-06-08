using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Models;
using SeguradosAPI.Services;

namespace SeguradosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: api/Usuario
        [HttpGet]
        public IActionResult Get()
        {
            var users = _service.ObterTodos();
            if (users.Count != 0)
                return Ok(users);

            return NoContent();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.ObterPorId(id);
            if (user != null)
                return Ok(user);

            return NoContent();
        }

        // POST: api/Usuario
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioModel usuario)
        {
            if (_service.Add(usuario))
                return Ok(usuario);

            return null;
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] UsuarioModel usuario)
        {
            if (_service.Atualizar(usuario))
                return Ok(usuario);

            return null;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => _service.Remover(id) ? Ok() : null;
    }
}
