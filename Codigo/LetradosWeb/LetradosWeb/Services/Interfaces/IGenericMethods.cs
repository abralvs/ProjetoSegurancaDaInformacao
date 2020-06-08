using System.Collections.Generic;

namespace SeguradosAPI.Services
{
    interface IGenericMethods<T>
    {
        bool Add(T objeto);
        bool Atualizar(T objeto);
        bool Remover(int id);
        List<T> ObterTodos();
        T ObterPorId(int id);
    }
}
