using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeguradosAPI.Models;
using SeguradosAPI.Services;

namespace LetradosWeb.Controllers
{
    public class TemaController : Controller
    {
        private readonly ITematicaService _service;
        private readonly IPerguntaService _serviceP;
        public TemaController(ITematicaService service, IPerguntaService serviceP)
        {
            _service = service;
            _serviceP = serviceP;
        }

        public List<TematicaModel> Temas()
        {
            var tematicas = _service.ObterTodos();
            return tematicas;
        }

        // POST: Tema/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUp(IFormCollection collection)
        {
            var id = collection["id-tema"];
            var nome = collection["nome-tema"];
            var descricao = collection["descricao-tema"];
            //update
            if (!id.Equals("-1"))
            {
                TematicaModel model = new TematicaModel
                {
                    IdTematica = Convert.ToInt32(id),
                    Titulo = nome,
                    Descricao = descricao
                };

                if (_service.Atualizar(model))
                    return RedirectToAction("Index", "Home", new { msg = "SucessoAttTema" });
            }
            //create
            else
            {
                TematicaModel model = new TematicaModel
                {
                    Titulo = nome,
                    Descricao = descricao
                };

                if (_service.Add(model))
                    return RedirectToAction("Index", "Home", new { msg = "SucessoAddTema" });
            }

            return RedirectToAction("Index", "Home", new { msg = "FalhaTema" });
        }
    
        // GET: Tema/Edit/5
        public TematicaModel Edit(int id)
        {
            var tematica = _service.ObterPorId(id);
            return tematica;
        }

        // GET: Tema/Delete/5
        public ActionResult Delete(int id)
        {
            var tematica = _serviceP.ObterPorTema(id);
            ///tem perguntas ass
            if (tematica != null)
                return RedirectToAction("Index", "Home", new { msg = "FalhaDelTema" });
            else
                return RedirectToAction("Index", "Home", new { msg = "SucessoDelTema" });
        }
    }
}