using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public partial class PerguntaController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewPergunta> perguntas = null;

            using (ctx = new JogoMasterEntities())
            {
                perguntas = ctx.Perguntas.Select(s => new ViewPergunta()
                {
                    Id = s.Id,
                    Pergunta = s.Pergunta1,
                    IdNivel = s.IdNivel,
                    IdTema = s.IdTema,
                    Patrocinada = s.Patrocinada
                }).ToList();
            }

            if (perguntas.Count == 0) return NotFound();

            return Ok(perguntas);
        }

        public IHttpActionResult Get(int id)
        {
            ViewPergunta pergunta = null;

            using (ctx = new JogoMasterEntities())
            {
                pergunta = ctx.Perguntas.Where(x => x.Id == id).Select(s => new ViewPergunta()
                {
                    Id = s.Id,
                    Pergunta = s.Pergunta1,
                    IdNivel = s.IdNivel,
                    IdTema = s.IdTema,
                    Patrocinada = s.Patrocinada
                }).FirstOrDefault();
            }

            if (pergunta == null) return NotFound();

            return Ok(pergunta);
        }

        public IHttpActionResult Post(ViewPergunta dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            ValidaPergunta(dados);
           
            using (ctx = new JogoMasterEntities())
            {
                ctx.Perguntas.Add(new Pergunta()
                {
                    Pergunta1 = dados.Pergunta,
                    Patrocinada = dados.Patrocinada,
                    IdTema = dados.IdTema,
                    IdNivel = dados.IdNivel,
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewPergunta dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var PerguntaAtual = ctx.Perguntas.Where(t => t.Id == dados.Id)
                                                        .FirstOrDefault();

                if (PerguntaAtual != null)
                {
                    PerguntaAtual.Pergunta1 = dados.Pergunta;
                    PerguntaAtual.Patrocinada = dados.Patrocinada;
                    PerguntaAtual.IdTema = dados.IdTema;
                    PerguntaAtual.IdNivel = dados.IdNivel;
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
            if (id <= 0) return BadRequest("ID inválido.");

            using (ctx = new JogoMasterEntities())
            {
                var pergunta = ctx.Perguntas
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (pergunta == null) return BadRequest("ID inválido");

                ctx.Entry(pergunta).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
