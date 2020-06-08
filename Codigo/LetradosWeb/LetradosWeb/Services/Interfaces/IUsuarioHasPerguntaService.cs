using SeguradosAPI.Models;
using System.Collections.Generic;

namespace SeguradosAPI.Services
{
    public interface IUsuarioHasPerguntaService
    {
        bool Add(UsuarioHasPerguntaModel objeto);
        bool Atualizar(UsuarioHasPerguntaModel objeto);
        List<UsuarioHasPerguntaModel> ObterPorId(int idUsuario);
        List<UsuarioHasPerguntaModel> ObterTodos();
        bool Remover(int idUsuario, int idPergunta);
    }
}