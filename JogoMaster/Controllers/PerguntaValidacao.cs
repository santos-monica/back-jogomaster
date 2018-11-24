using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class PerguntaController : Validacao
    {
        private void ValidaDadosConsulta(ConsultaNivelTemas dados)
        {
            using (ctx = new JogoMasterEntities())
            {
                dados.idsTema.ForEach(idTema =>
                {
                    Refute(idTema <= 0, "Tema inválido.");

                    Tema tema = null;
                    tema = ctx.Temas
                        .FirstOrDefault(x => x.Id == idTema);
                    Refute(tema == null, "Tema inexistente.");
                });

                Nivel nivel = null;
                nivel = ctx.Niveis.Where(x => x.Id == dados.idNivel)
                    .FirstOrDefault();
                Refute(nivel == null, "Nível inexistente.");
                Refute(dados.idNivel <= 0, "Nível inválido.");
            }
        }

        private void ValidaPergunta(ViewPergunta dados, List<string> erros)
        {
            Refute(dados.IdTema <= 0, "Informe o Tema da Pergunta.", erros);
            Refute(dados.IdNivel <= 0, "Informe o Nível da Pergunta.", erros);
            Refute(string.IsNullOrEmpty(dados.Pergunta), "Informe a Pergunta.", erros);

            using (ctx = new JogoMasterEntities())
            {
                Nivel nivel = null;
                nivel = ctx.Niveis.Where(x => x.Id == dados.IdNivel)
                    .FirstOrDefault();
                Refute(nivel == null, "Nível inexistente.", erros);

                Tema tema = null;
                tema = ctx.Temas
                    .FirstOrDefault(x => x.Id == dados.IdTema);
                Refute(tema == null, "Tema inexistente.", erros);

                Pergunta pergunta = null;
                pergunta = ctx.Perguntas
                    .Where(x => x.Pergunta1.ToLower() == dados.Pergunta.ToLower())
                    .FirstOrDefault();
                Refute(pergunta != null, $"Pergunta já cadastrada \"{ pergunta?.Pergunta1}\".", erros);
            }
        }

        private void ValidaResposta(List<ViewResposta> dados, List<string> erros)
        {
            var certas = dados.Count(res => res.Correta == true);
            Refute(certas != 1, "Somente uma resposta deve ser a correta.", erros);
            dados.ForEach(res => { Refute(string.IsNullOrEmpty(res.Resposta), "Informe a Resposta.", erros); });
        }
    }
}