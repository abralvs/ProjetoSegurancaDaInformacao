using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Models;
using SeguradosAPI.Models.ViewModel;
using SeguradosAPI.Services;

/// <summary>
/// Controladora para retornos os dados dos usuarios / estatisticos e o ranking 
/// </summary>

namespace SeguradosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class EstatisticaController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioHasPerguntaService _hasPerguntaService;
        private readonly IPerguntaService _perguntaService;
        private readonly ITematicaService _tematicaService;

        public EstatisticaController(IUsuarioService usuarioService, IUsuarioHasPerguntaService hasPerguntaService, IPerguntaService perguntaService, ITematicaService tematicaService)
        {
            _usuarioService = usuarioService;
            _hasPerguntaService = hasPerguntaService;
            _perguntaService = perguntaService;
            _tematicaService = tematicaService;
        }
        // GET: api/Estatistica get ranking
        [HttpGet]
        public IActionResult Get()
        {
            var usuarios = _usuarioService.ObterTodos();
            List<RankingViewModel> ranking = new List<RankingViewModel>();
            foreach(var user in usuarios)
            {
                var hasPerguntas = _hasPerguntaService.ObterTodos().Where(r => r.IdUsuario == user.IdUsuario);
                var perguntasUsuario =
                    from perg in _perguntaService.ObterTodos()
                    join hasP in hasPerguntas on perg.IdPergunta equals hasP.IdPergunta  where (hasP.Acertou == 1)
                    select new
                    {
                        pontuacao = perg.Pontuacao
                    };

                ranking.Add
                    (
                        new RankingViewModel
                        {
                            NomeUsuario = user.Nome,
                            Pontos = perguntasUsuario.Sum(x => Int32.Parse(x.pontuacao)),
                            Perfil = user.Perfil
                        }
                    );
            }

            if (ranking != null)
            {
                ranking = ranking.OrderByDescending(s=> s.Pontos).ToList();
                return Ok(ranking);
            }

            return NoContent();
        }

        // GET: api/Estatistica/5 by id user 
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var usuario = _usuarioService.ObterPorId(id);
            var tematica = _tematicaService.ObterTodos();
            var perguntasId = _hasPerguntaService.ObterTodos().Where(r => r.IdUsuario == id);
            var qtd = perguntasId.Count();
            var perguntasUsuario =
                 (from perg in _perguntaService.ObterTodos()
                  join hasP in perguntasId on perg.IdPergunta equals hasP.IdPergunta
                  where (hasP.Acertou == 1)
                  select new
                  {
                      idPergunta = perg.IdPergunta,
                      idTematica = perg.TematicaIdTematica,
                      pontos = perg.Pontuacao
                  });
            var pontosTematica = (from perg in perguntasUsuario
                                  join tema in tematica on perg.idTematica equals tema.IdTematica
                                  select new PontosPorTematicaViewModel

                                  {
                                      Pontos = Int32.Parse(perg.pontos),
                                      TematicaModel = tema
                                  }).GroupBy(x => x.TematicaModel.IdTematica).Select(y => new PontosPorTematicaViewModel
                                  {
                                     Pontos = y.Sum(z => z.Pontos),
                                     TematicaModel = y.First().TematicaModel
                                  }).ToList();
 
            if(pontosTematica != null)
            return Ok(pontosTematica);
            return NoContent();
        }

        // POST: api/Estatistica
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Estatistica/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
