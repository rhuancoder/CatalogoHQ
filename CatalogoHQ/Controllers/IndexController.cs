using CatalogoHQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CatalogoHQ.Controllers
{
    public class IndexController : Controller
    {

        public IActionResult Index([FromServices]IConfiguration configuracao, [FromServices]IMemoryCache cache)
        {
            var personagemController = new PersonagemController();

            ViewBag.Personagens = new SelectList(personagemController.ObterPersonagens(configuracao, cache), "Id", "Nome", null);
            return View();
        }

        #region Json Requests
        public string RetornaPersonagem([FromServices]IConfiguration configuracao, int id)
        {
            var personagemController = new PersonagemController();
            var personagem = personagemController.ObterPersonagem(configuracao, id);

            return JsonConvert.SerializeObject(personagem);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
