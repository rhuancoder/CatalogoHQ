using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CatalogoHQ.Controllers
{
    public class HashController : Controller
    {
        public HashController()
        {

        }

        public string GerarHash(string ts, string chavePublica, string chavePrivada)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(ts + chavePrivada + chavePublica);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);

            return BitConverter.ToString(bytesHash).ToLower().Replace("-", "");
        }

    }
}
