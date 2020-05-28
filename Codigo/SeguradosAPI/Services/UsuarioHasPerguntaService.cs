using SeguradosAPI.Models;
using SeguradosAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeguradosAPI.Services
{
    public class UsuarioHasPerguntaService : IUsuarioHasPerguntaService
    {
        private readonly DBContext _context;
        public UsuarioHasPerguntaService(DBContext context)
        {
            _context = context;
        }

        public bool Add(UsuarioHasPerguntaModel objeto)
        {
            if (objeto != null)
            {
                _context.Add(ModelToEntity(objeto, new UsuarioHasPergunta()));
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        public bool Atualizar(UsuarioHasPerguntaModel objeto)
        {
            if (objeto != null)
            {
                var antigo = _context.UsuarioHasPergunta.Where(r => r.IdUsuario == objeto.IdUsuario && r.IdPergunta == objeto.IdPergunta).FirstOrDefault();
                if (antigo != null)
                {
                    _context.Update(ModelToEntity(objeto, antigo));
                    return _context.SaveChanges() == 1 ? true : false;
                }
                return false;
            }
            return false;
        }

        public List<UsuarioHasPerguntaModel> ObterPorId(int idUsuario)
                   => _context
                       .UsuarioHasPergunta
                       .Select(r => new UsuarioHasPerguntaModel
                       {
                           IdPergunta = r.IdPergunta,
                           Acertou = r.Acertou,
                           IdUsuario = r.IdUsuario
                       }).Where(s => s.IdUsuario == idUsuario) .ToList();

        public List<UsuarioHasPerguntaModel> ObterTodos()
            => _context
                .UsuarioHasPergunta
                .Select(r => new UsuarioHasPerguntaModel
                {
                    IdPergunta = r.IdPergunta,
                    Acertou = r.Acertou,
                    IdUsuario = r.IdUsuario
                }).ToList();

        public bool Remover(int idUsuario, int idPergunta)
        {
            var obj = _context.UsuarioHasPergunta.Where(r => r.IdUsuario == idUsuario && r.IdPergunta == idPergunta).FirstOrDefault();
            if (obj != null)
            {
                _context.Remove(obj);
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        private UsuarioHasPergunta ModelToEntity(UsuarioHasPerguntaModel model, UsuarioHasPergunta entity)
        {
            entity.IdUsuario = model.IdUsuario;
            entity.IdPergunta = model.IdPergunta;
            entity.Acertou = Convert.ToByte(model.Acertou);
            return entity;
        }
    }
}
