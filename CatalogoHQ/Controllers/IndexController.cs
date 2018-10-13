using CatalogoHQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CatalogoHQ.Controllers
{
    public class IndexController : Controller
    {
        

        public IActionResult Index([FromServices]IConfiguration configuracao)
        {
            var personagemController = new PersonagemController();

            ViewBag.Personagens = new SelectList(personagemController.ObterPersonagens(configuracao), "Id", "Nome");
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
