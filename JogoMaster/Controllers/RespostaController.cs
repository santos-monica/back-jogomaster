using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    [Authorize]
    public partial class RespostaController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewResposta> respostas = null;

            using (ctx = new JogoMasterEntities())
            {
                respostas = ctx.Respostas.Select(s => new ViewResposta()
                {
                    Id = s.Id,
                    Resposta = s.Resposta1,
                    Correta = s.Correta,
                    IdPergunta = s.IdPergunta
                }).ToList();
            }

            if (respostas.Count == 0) return NotFound();

            return Ok(respostas);
        }

        public IHttpActionResult Get(int id)
        {
            ViewResposta resposta = null;

            using (ctx = new JogoMasterEntities())
            {
                resposta = ctx.Respostas.Where(x => x.Id == id).Select(s => new ViewResposta()
                {
                    Id = s.Id,
                    Correta = s.Correta,
                    Resposta = s.Resposta1,
                    IdPergunta = s.IdPergunta
                }).FirstOrDefault();
            }

            if (resposta == null) return NotFound();

            return Ok(resposta);
        }

        public IHttpActionResult Post(ViewResposta dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                ctx.Respostas.Add(new Resposta()
                {
                    Correta = dados.Correta,
                    Resposta1 = dados.Resposta,
                    IdPergunta = dados.IdPergunta
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewResposta dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var RespostaAtual = ctx.Respostas.Where(t => t.Id == dados.Id)
                                                        .FirstOrDefault<Resposta>();

                if (RespostaAtual != null)
                {
                    RespostaAtual.Correta = dados.Correta;
                    RespostaAtual.Resposta1 = dados.Resposta;
                    RespostaAtual.IdPergunta = dados.IdPergunta;
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
                var resposta = ctx.Respostas
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (resposta == null) return BadRequest("ID inválido");

                ctx.Entry(resposta).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
