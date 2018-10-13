using CatalogoHQ.Models;
using CatalogoHQ.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CatalogoHQ.Controllers
{
    public class PersonagemController : Controller
    {
       
        public PersonagemController()
        {

        }

        public List<Personagem> ObterPersonagens(IConfiguration configuracao)
        {
            var repositorio = new PersonagemRepositorio(configuracao);

            return repositorio.ObterPersonagens(configuracao);
        }
    }
}
