using CatalogoHQ.Controllers;
using CatalogoHQ.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CatalogoHQ.Repository
{
    public class QuadrinhoRepositorio
    {
        HttpClient cliente = new HttpClient();
        private string chavePublica;
        private string chavePrivada;
        private string hash;
        private string ts;

        public QuadrinhoRepositorio(IConfiguration configuracao)
        {
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ts = DateTime.Now.Ticks.ToString();
            chavePublica = configuracao.GetSection("MarvelComicsAPI:PublicKey").Value;
            chavePrivada = configuracao.GetSection("MarvelComicsAPI:PrivateKey").Value;

            var hashController = new HashController();
            hash = hashController.GerarHash(ts, chavePublica, chavePrivada);
        }

        public int ObterTotalQuadrinhos(IConfiguration configuracao, int idPersonagem)
        {
            HttpResponseMessage reposta = cliente.GetAsync(
                configuracao.GetSection("MarvelComicsAPI:RequestURL").Value +
                $"characters/{idPersonagem}/comics?ts={ts}&apikey={chavePublica}&hash={hash}&limit={1}&offset={1}").Result;

            reposta.EnsureSuccessStatusCode();
            string conteudo = reposta.Content.ReadAsStringAsync().Result;

            dynamic resultado = JsonConvert.DeserializeObject(conteudo);

            return resultado.data.total;
        }

        public List<Quadrinho> ObterQuadrinhos(IConfiguration configuracao, int idPersonagem)
        {
            var total = ObterTotalQuadrinhos(configuracao, idPersonagem);
            var max = (int)Enums.Enums.LimiteQuadrinho.Maximo;
            var lsQuadrinhos = new List<Quadrinho>();

            while (total > lsQuadrinhos.Count)
            {
                HttpResponseMessage resposta = cliente.GetAsync(
                        configuracao.GetSection("MarvelComicsAPI:RequestURL").Value +
                        $"characters/{idPersonagem}/comics?ts={ts}&apikey={chavePublica}&hash={hash}&limit={max}&offset={lsQuadrinhos.Count}").Result;

                resposta.EnsureSuccessStatusCode();
                string conteudo = resposta.Content.ReadAsStringAsync().Result;

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                for (var x = 0; x < (int)resultado.data.count; x++)
                {
                    Quadrinho quadrinho = new Quadrinho
                    {
                        Id = resultado.data.results[x].id,
                        Titulo = resultado.data.results[x].title
                    };

                    lsQuadrinhos.Add(quadrinho);
                }

            }
            return lsQuadrinhos;
        }

        public Quadrinho ObterQuadrinho(IConfiguration configuracao, int id)
        {
            HttpResponseMessage reposta = cliente.GetAsync(
             configuracao.GetSection("MarvelComicsAPI:RequestURL").Value +
             $"comics/{id}?ts={ts}&apikey={chavePublica}&hash={hash}").Result;

            reposta.EnsureSuccessStatusCode();
            string conteudo = reposta.Content.ReadAsStringAsync().Result;

            dynamic resultado = JsonConvert.DeserializeObject(conteudo);

            Quadrinho quadrinho = new Quadrinho
            {
                Id = resultado.data.results[0].id,
                Titulo = resultado.data.results[0].title,
                Descricao = resultado.data.results[0].description,
                ImagemUrl = resultado.data.results[0].thumbnail.path + "/portrait_xlarge." + resultado.data.results[0].thumbnail.extension
            };

            return quadrinho;
        }
    }
}
