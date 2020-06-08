using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LetradosWeb.Models;
using Microsoft.AspNetCore.Authorization;
using LetradosWeb.Models.ViewModel;
using Microsoft.AspNetCore.Http;

namespace LetradosWeb.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionModel.SessionUser) == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
