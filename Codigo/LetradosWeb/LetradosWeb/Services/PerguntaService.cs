using SeguradosAPI.Models;
using SeguradosAPI.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace SeguradosAPI.Services
{
    public class PerguntaService : IPerguntaService
    {
        private readonly DBContext _context;
        public PerguntaService(DBContext context)
        {
            _context = context;
        }
        public bool Add(PerguntaModel objeto)
        {
            if (objeto != null)
            {
                _context.Add(ModelToEntity(objeto, new Pergunta()));
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        public bool Atualizar(PerguntaModel objeto)
        {
            if (objeto != null)
            {
                var antigo = _context.Pergunta.Where(r => r.IdPergunta == objeto.IdPergunta).FirstOrDefault();
                if (antigo != null)
                {
                    _context.Update(ModelToEntity(objeto, antigo));
                    return _context.SaveChanges() == 1 ? true : false;
                }
                return false;
            }
            return false;
        }

        public PerguntaModel ObterPorId(int id)
            => _context
                .Pergunta
                .Where(r => r.IdPergunta == id)
                .Select(r => new PerguntaModel
                {
                    IdPergunta = r.IdPergunta,
                    OpcaoCorreta = r.OpcaoCorreta,
                    Alternativa1 = r.Alternativa1,
                    Alternativa2 = r.Alternativa2,
                    Alternativa3 = r.Alternativa3,
                    Alternativa4 = r.Alternativa4,
                    Dificuldade = r.Dificuldade,
                    Pontuacao = r.Pontuacao,
                    Questao = r.Questao,
                    TematicaIdTematica = r.TematicaIdTematica,
                    Tempo = r.Tempo
                }).FirstOrDefault();

        public List<PerguntaModel> ObterTodos()
            => _context
                .Pergunta
                .Select(r => new PerguntaModel
                {
                    IdPergunta = r.IdPergunta,
                    OpcaoCorreta = r.OpcaoCorreta,
                    Alternativa1 = r.Alternativa1,
                    Alternativa2 = r.Alternativa2,
                    Alternativa3 = r.Alternativa3,
                    Alternativa4 = r.Alternativa4,
                    Dificuldade = r.Dificuldade,
                    Pontuacao = r.Pontuacao,
                    Questao = r.Questao,
                    TematicaIdTematica = r.TematicaIdTematica,
                    Tempo = r.Tempo
                }).ToList();

        public List<PerguntaModel> ObterPorTema(int idTema)
    => _context
        .Pergunta
        .Where(m => m.TematicaIdTematica == idTema)
        .Select(r => new PerguntaModel
        {
            IdPergunta = r.IdPergunta,
            OpcaoCorreta = r.OpcaoCorreta,
            Alternativa1 = r.Alternativa1,
            Alternativa2 = r.Alternativa2,
            Alternativa3 = r.Alternativa3,
            Alternativa4 = r.Alternativa4,
            Dificuldade = r.Dificuldade,
            Pontuacao = r.Pontuacao,
            Questao = r.Questao,
            TematicaIdTematica = r.TematicaIdTematica,
            Tempo = r.Tempo
        }).ToList();

        public List<PerguntaModel> ObterPorUsuario(int idUsuario)
           => _context
               .Pergunta
               .Select(r => new PerguntaModel
               {
                   IdPergunta = r.IdPergunta,
                   OpcaoCorreta = r.OpcaoCorreta,
                   Alternativa1 = r.Alternativa1,
                   Alternativa2 = r.Alternativa2,
                   Alternativa3 = r.Alternativa3,
                   Alternativa4 = r.Alternativa4,
                   Dificuldade = r.Dificuldade,
                   Pontuacao = r.Pontuacao,
                   Questao = r.Questao,
                   TematicaIdTematica = r.TematicaIdTematica,
                   Tempo = r.Tempo
               }).ToList();

        public bool Remover(int id)
        {
            var obj = _context.Pergunta.Where(m => m.IdPergunta == id).FirstOrDefault();
            if (obj != null)
            {
                _context.Remove(obj);
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        private Pergunta ModelToEntity(PerguntaModel model, Pergunta entity)
        {
            entity.IdPergunta = model.IdPergunta;
            entity.Questao = model.Questao;
            entity.Alternativa1 = model.Alternativa1;
            entity.Alternativa2 = model.Alternativa2;
            entity.Alternativa3 = model.Alternativa3;
            entity.Alternativa4 = model.Alternativa4;
            entity.OpcaoCorreta = model.OpcaoCorreta;
            entity.Pontuacao = model.Pontuacao;
            entity.Tempo = model.Tempo;
            entity.Dificuldade = model.Dificuldade;
            entity.TematicaIdTematica = model.TematicaIdTematica;

            return entity;
        }
    }
}
