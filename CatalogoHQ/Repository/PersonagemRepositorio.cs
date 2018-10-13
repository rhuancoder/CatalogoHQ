using CatalogoHQ.Controllers;
using CatalogoHQ.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CatalogoHQ.Repository
{
    public class PersonagemRepositorio
    {
        HttpClient cliente = new HttpClient();
        private string chavePublica;
        private string chavePrivada;
        private string hash;
        private string ts;

        public PersonagemRepositorio(IConfiguration configuracao)
        {
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ts = DateTime.Now.Ticks.ToString();
            chavePublica = configuracao.GetSection("MarvelComicsAPI:PublicKey").Value;
            chavePrivada = configuracao.GetSection("MarvelComicsAPI:PrivateKey").Value;

            var hashController = new HashController();
            hash = hashController.GerarHash(ts, chavePublica, chavePrivada);
        }

        public int ObterTotalPersonagens(IConfiguration configuracao)
        {
            HttpResponseMessage reposta = cliente.GetAsync(
                configuracao.GetSection("MarvelComicsAPI:RequestURL").Value +
                $"characters?ts={ts}&apikey={chavePublica}&hash={hash}&limit={1}&offset={1}").Result;

            reposta.EnsureSuccessStatusCode();
            string conteudo = reposta.Content.ReadAsStringAsync().Result;

            dynamic resultado = JsonConvert.DeserializeObject(conteudo);

            return resultado.data.total;
        }

        public List<Personagem> ObterPersonagens(IConfiguration configuracao)
        {
            var lsPersonagens = new List<Personagem>();

            var total = ObterTotalPersonagens(configuracao);

            while (total > lsPersonagens.Count)
            {
                var max = (int)Enums.Enums.LimitePersonagem.Maximo;

                HttpResponseMessage response = cliente.GetAsync(
                    configuracao.GetSection("MarvelComicsAPI:RequestURL").Value +
                    $"characters?ts={ts}&apikey={chavePublica}&hash={hash}&limit={max}&offset={lsPersonagens.Count}").Result;

                response.EnsureSuccessStatusCode();
                string conteudo = response.Content.ReadAsStringAsync().Result;

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                for (var x = 0; x < (int)resultado.data.count; x++)
                {
                    Personagem personagem = new Personagem
                    {
                        Id = resultado.data.results[x].id,
                        Nome = resultado.data.results[x].name
                    };

                    lsPersonagens.Add(personagem);
                }

            }

            return lsPersonagens;
        }
    }
}
