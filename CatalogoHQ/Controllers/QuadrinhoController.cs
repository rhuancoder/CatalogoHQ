using CatalogoHQ.Models;
using CatalogoHQ.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CatalogoHQ.Controllers
{
    public class QuadrinhoController : Controller
    {
        public QuadrinhoController()
        {

        }

        public List<Quadrinho> ObterQuadrinhos(IConfiguration configuracao, int idPersonagem)
        {
            var repositorio = new QuadrinhoRepositorio(configuracao);

            return repositorio.ObterQuadrinhos(configuracao, idPersonagem);
        }

        public Quadrinho ObterQuadrinho(IConfiguration configuracao, int id)
        {
            var repositorio = new QuadrinhoRepositorio(configuracao);

            return repositorio.ObterQuadrinho(configuracao, id);
        }
    }
}
