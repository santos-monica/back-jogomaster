using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public partial class ClassificacaoController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewClassificacao> classificacoes = null;

            using (ctx = new JogoMasterEntities())
            {
                classificacoes = ctx.Classificacoes.Select(s => new ViewClassificacao()
                {
                    Id = s.Id,
                    Classificacao = s.Classificacao1,
                    PontuacaoMinima = s.PontuacaoMinima,
                    PontuacaoMaxima = s.PontuacaoMaxima,
                }).ToList();
            }

            if (classificacoes.Count == 0) return NotFound();

            return Ok(classificacoes);
        }

        public IHttpActionResult Get(int id)
        {
            ViewClassificacao classificacao = null;

            using (ctx = new JogoMasterEntities())
            {
                classificacao = ctx.Classificacoes.Where(x => x.Id == id).Select(s => new ViewClassificacao()
                {
                    Id = s.Id,
                    Classificacao = s.Classificacao1,
                    PontuacaoMinima = s.PontuacaoMinima,
                    PontuacaoMaxima = s.PontuacaoMaxima,
                }).FirstOrDefault();
            }

            if (classificacao == null) return NotFound();

            return Ok(classificacao);
        }

        public IHttpActionResult Post(ViewClassificacao dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            ValidaClassificacao(dados);

            using (ctx = new JogoMasterEntities())
            {
                ctx.Classificacoes.Add(new Classificacao()
                {
                    Classificacao1 = dados.Classificacao,
                    PontuacaoMinima = dados.PontuacaoMinima,
                    PontuacaoMaxima = dados.PontuacaoMaxima,
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewClassificacao dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var ClassificacaoAtual = ctx.Classificacoes.Where(t => t.Id == dados.Id)
                                                        .FirstOrDefault<Classificacao>();

                if (ClassificacaoAtual != null)
                {
                    ClassificacaoAtual.Classificacao1 = dados.Classificacao;
                    ClassificacaoAtual.PontuacaoMinima = dados.PontuacaoMinima;
                    ClassificacaoAtual.PontuacaoMaxima = dados.PontuacaoMaxima;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            using (ctx = new JogoMasterEntities())
            {
                var classificacao = ctx.Classificacoes
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (classificacao == null) return BadRequest("ID inválido");

                ctx.Entry(classificacao).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
