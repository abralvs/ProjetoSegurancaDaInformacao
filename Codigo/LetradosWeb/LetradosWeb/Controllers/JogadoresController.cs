using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetradosWeb.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Services;

namespace LetradosWeb.Controllers
{
    public class JogadoresController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioHasPerguntaService _hasPerguntaService;
        private readonly IPerguntaService _perguntaService;

        public JogadoresController(IUsuarioService usuarioService, IUsuarioHasPerguntaService hasPerguntaService, IPerguntaService perguntaService)
        {
            _usuarioService = usuarioService;
            _hasPerguntaService = hasPerguntaService;
            _perguntaService = perguntaService;
        }

        public List<RankingWebViewModel> Usuarios()
        {
            var usuarios = _usuarioService.ObterTodos();
            List<RankingWebViewModel> ranking = new List<RankingWebViewModel>();
            foreach (var user in usuarios)
            {
                var hasPerguntas = _hasPerguntaService.ObterTodos().Where(r => r.IdUsuario == user.IdUsuario);
                var perguntasUsuario =
                    from perg in _perguntaService.ObterTodos()
                    join hasP in hasPerguntas on perg.IdPergunta equals hasP.IdPergunta
                    where (hasP.Acertou == 1)
                    select new
                    {
                        pontuacao = perg.Pontuacao
                    };

                if (user.Usuario.Equals("luciana"))
                    user.Nome = user.Nome + " * ADM";

                ranking.Add
                    (
                        new RankingWebViewModel
                        {
                            NomeUsuario = user.Nome,
                            Pontos = perguntasUsuario.Sum(x => Int32.Parse(x.pontuacao)),
                            Id = user.IdUsuario
                        }
                    );
            }


            ranking = ranking.OrderByDescending(s => s.Pontos).ToList();
            return ranking;

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}