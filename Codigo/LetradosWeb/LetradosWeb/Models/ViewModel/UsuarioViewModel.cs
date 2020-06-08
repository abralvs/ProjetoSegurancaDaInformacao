using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeguradosAPI.Models.ViewModel
{
    public class UsuarioViewModel
    {
        public UsuarioModel UsuarioModel { get; set; }
        public int QtdPerguntas { get; set; }
        public PontosPorTematicaViewModel PontosTematica { get; set; }
    }
}
