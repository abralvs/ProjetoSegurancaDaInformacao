using System;
using System.Collections.Generic;

namespace SeguradosAPI
{
    public partial class UsuarioHasPergunta
    {
        public int IdUsuario { get; set; }
        public int IdPergunta { get; set; }
        public byte Acertou { get; set; }

        public Pergunta IdPerguntaNavigation { get; set; }
        public Usuario IdUsuarioNavigation { get; set; }
    }
}
