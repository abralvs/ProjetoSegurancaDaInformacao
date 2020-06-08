using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SeguradosAPI.Models;
using SeguradosAPI.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeguradosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IUsuarioService _service;
        public AuthenticateController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Ola mundo");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult RequestToken([FromBody] UsuarioModel model) {

            var user = _service.ObterPorUsuarioSenha(model.Usuario, model.Senha);
            if (user != null) {

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name,model.Nome)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("buy43g54389h8rhr874y4r387264tr743568754"));

                var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken(
                                    issuer: "Segurados.net",
                                    audience: "Segurados.net",
                                    claims: claims,
                                    expires: DateTime.Now.AddDays(30),
                                    signingCredentials: creds);

                return Ok(new
                {
                    user.IdUsuario,
                    user.Nome,
                    user.Perfil,
                    user.Usuario,
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Credenciais inválidas");
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UsuarioModel model)
        {

            var user = _service.ObterPorUsuario(model.Usuario);
            if (user == null)
            {
                if (_service.Add(model))
                    return Ok(model);
            }

            return BadRequest("Esse nome de usuário já está sendo utilizado.");
        }

    }
}
