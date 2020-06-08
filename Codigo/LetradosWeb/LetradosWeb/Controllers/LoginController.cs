using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LetradosWeb.Models;
using LetradosWeb.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SeguradosAPI.Models;
using SeguradosAPI.Services;

namespace LetradosWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _service;
        public LoginController(IUsuarioService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionModel.SessionUser) != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Logar(LoginModel model)
        {
            var user = _service.ObterPorUsuarioSenha(model.Usuario, model.Senha);
            if (user != null)
            {
                if (!user.Usuario.Equals("luciana")) // trocar esse pra luciana dps - armengaço                var claims = new[]
                    return RedirectToAction("Index", "Login", new { msg = "denied" });

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, model.Usuario)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("buy43g54389h8rhr874y4r387264tr743568754"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken(
                                    issuer: "Segurados.net",
                                    audience: "Segurados.net",
                                    claims: claims,
                                    expires: DateTime.Now.AddDays(30),
                                    signingCredentials: creds);

                string bearer = new JwtSecurityTokenHandler().WriteToken(token);

                HttpContext.Session.SetString(SessionModel.SessionName, user.Nome);
                HttpContext.Session.SetString(SessionModel.SessionUser, user.Usuario);
                HttpContext.Session.SetString(SessionModel.SessionToken, bearer);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Login", new { msg = "error" });
        }

        public IActionResult Deslogar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}