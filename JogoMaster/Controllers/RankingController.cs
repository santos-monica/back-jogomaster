using JogoMaster.Models;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    //[Authorize]
    public class RankingController : ApiController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            List<ViewRanking> pontuacaoJogadores = null;

            using (ctx = new JogoMasterEntities())
            {
                pontuacaoJogadores = ctx.Usuarios.Select(s => new ViewRanking()
                {
                    IdUsuario = s.Id,
                    Username = s.Username,
                    PontosJogada = s.Pontos
                }).
                OrderBy(x => x.PontosJogada)
                .ToList();

                pontuacaoJogadores.ForEach(jogador =>
                {
                    jogador.Classificacao = ctx.Classificacoes
                    .Where(c => jogador.PontosJogada >= c.PontuacaoMinima && jogador.PontosJogada <= c.PontuacaoMaxima)
                    .Select(s => s.Classificacao1)
                    .FirstOrDefault();
                });
            }

            if (pontuacaoJogadores.Count == 0) return NotFound();

            return Ok(pontuacaoJogadores);
        }

    }
}
