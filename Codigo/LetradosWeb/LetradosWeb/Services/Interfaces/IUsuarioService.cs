using SeguradosAPI.Models;
using System.Collections.Generic;

namespace SeguradosAPI.Services
{
    public interface IUsuarioService
    {
        bool Add(UsuarioModel objeto);
        bool Atualizar(UsuarioModel objeto);
        UsuarioModel ObterPorId(int id);
        List<UsuarioModel> ObterTodos();
        bool Remover(int id);
        UsuarioModel ObterPorUsuarioSenha(string email, string senha);
        UsuarioModel ObterPorUsuario(string email);
    }
}