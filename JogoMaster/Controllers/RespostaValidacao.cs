using JogoMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Controllers
{
    public partial class RespostaController : Validacao
    {
        private void ValidaResposta(ViewResposta dados)
        {
            Refute(string.IsNullOrEmpty(dados.Resposta), "Informe a Resposta.");
            using(ctx = new JogoMasterEntities())
            {
                Pergunta pergunta = null;
                pergunta = ctx.Perguntas.FirstOrDefault(x => x.Id == dados.IdPergunta);
                Refute(pergunta == null, "Id da Pergunta inválido.");

                Resposta resposta = null;
                resposta = ctx.Respostas.FirstOrDefault(x => x.Resposta1.ToLower() == dados.Resposta.ToLower() && x.IdPergunta == dados.IdPergunta);
                Refute(resposta != null, "Resposta já cadastrada.");
            }
        }
    }
}