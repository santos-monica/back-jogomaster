using JogoMaster.Models;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class PerguntaController : Validacao
    {
        private void ValidaPergunta(ViewPergunta dados)
        {
            Refute(dados.IdTema <= 0, "Informe o Tema da Pergunta.");
            Refute(dados.IdNivel <= 0, "Informe o Nível da Pergunta.");
            Refute(string.IsNullOrEmpty(dados.Pergunta), "Informe a Pergunta.");

            using (ctx = new JogoMasterEntities())
            {
                Nivel nivel = null;
                nivel = ctx.Niveis
                    .FirstOrDefault(x => x.Id == dados.IdNivel);
                Refute(nivel == null, "Nível inexistente.");

                Tema tema = null;
                tema = ctx.Temas
                    .FirstOrDefault(x => x.Id == dados.IdTema);
                Refute(tema == null, "Tema inexistente.");

                Pergunta pergunta = null;
                pergunta = ctx.Perguntas
                    .Where(x => x.Pergunta1.ToLower() == dados.Pergunta.ToLower())
                    .FirstOrDefault();
                Refute(pergunta != null, "Pergunta já cadastrada.");
            }
        }
    }
}