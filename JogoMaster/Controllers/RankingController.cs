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

        public IHttpActionResult Get(int id)
        {
            List<ViewRanking> pontuacaoJogadores = null;
            var ranking = new retornoRanking();
            using (ctx = new JogoMasterEntities())
            {
                pontuacaoJogadores = ctx.Usuarios.Where(x => x.Cadastrado == true).Select(s => new ViewRanking()
                {
                    IdUsuario = s.Id,
                    Username = s.Username,
                    PontosJogada = s.Pontos,
                    Skin = s.Skin
                }).
                OrderByDescending(x => x.PontosJogada)
                .ToList();

                int index = pontuacaoJogadores.IndexOf(pontuacaoJogadores.Where(p => p.IdUsuario == id).FirstOrDefault()) + 1;

                var pontuacao = pontuacaoJogadores.Take(10).ToList();
                var ordem = 1;
                pontuacao.ForEach(jogador =>
                {
                    jogador.Posicao = ordem;
                    ordem++;
                    jogador.Classificacao = ctx.Classificacoes
                    .Where(c => jogador.PontosJogada >= c.PontuacaoMinima && jogador.PontosJogada <= c.PontuacaoMaxima)
                    .Select(s => s.Classificacao1)
                    .FirstOrDefault();
                });

                ranking.ranking = pontuacao;
                ranking.usuario = ctx.Usuarios.Where(x => x.Id == id).Select(x => new ViewRanking
                {
                    Posicao = index,
                    IdUsuario = x.Id,
                    PontosJogada = x.Pontos,
                    Username = x.Username,
                    Skin = x.Skin
                }).FirstOrDefault();

                ranking.usuario.Classificacao = ctx.Classificacoes
                    .Where(c => ranking.usuario.PontosJogada >= c.PontuacaoMinima && ranking.usuario.PontosJogada <= c.PontuacaoMaxima)
                    .Select(s => s.Classificacao1)
                    .FirstOrDefault();

            }
            
            return Ok(ranking);
        }

    }
}
