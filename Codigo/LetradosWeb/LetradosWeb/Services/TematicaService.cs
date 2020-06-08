using SeguradosAPI.Models;
using SeguradosAPI.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace SeguradosAPI.Services
{
    public class TematicaService : ITematicaService
    {
        private readonly DBContext _context;
        public TematicaService(DBContext context)
        {
            _context = context;
        }

        public bool Add(TematicaModel objeto)
        {
            if (objeto != null)
            {
                _context.Add(ModelToEntity(objeto, new Tematica()));
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        public bool Atualizar(TematicaModel objeto)
        {
            if (objeto != null)
            {
                var antigo = _context.Tematica.Where(r => r.IdTematica == objeto.IdTematica).FirstOrDefault();
                if (antigo != null)
                {
                    _context.Update(ModelToEntity(objeto, antigo));
                    return _context.SaveChanges() == 1 ? true : false;
                }
                return false;
            }
            return false;
        }

        public TematicaModel ObterPorId(int id)
            => _context
                .Tematica
                .Where(r => r.IdTematica == id)
                .Select(r => new TematicaModel
                {
                    IdTematica = r.IdTematica,
                    Descricao = r.Descricao,
                    Titulo = r.Titulo
                }).FirstOrDefault();

        public List<TematicaModel> ObterTodos()
            => _context
                .Tematica
                .Select(r => new TematicaModel
                {
                    IdTematica = r.IdTematica,
                    Descricao = r.Descricao,
                    Titulo = r.Titulo
                }).ToList();

        public bool Remover(int id)
        {
            var obj = _context.Tematica.Where(m => m.IdTematica == id).FirstOrDefault();
            if (obj != null)
            {
                _context.Remove(obj);
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        private Tematica ModelToEntity(TematicaModel model, Tematica entity)
        {
            entity.IdTematica = model.IdTematica;
            entity.Titulo = model.Titulo;
            entity.Descricao = model.Descricao;

            return entity;
        }
    }
}
