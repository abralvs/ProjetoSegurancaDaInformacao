 using SeguradosAPI.Models;
using System.Collections.Generic;

namespace SeguradosAPI.Services
{
    public interface ITematicaService
    {
        bool Add(TematicaModel objeto);
        bool Atualizar(TematicaModel objeto);
        TematicaModel ObterPorId(int id);
        List<TematicaModel> ObterTodos();
        bool Remover(int id);
    }
}