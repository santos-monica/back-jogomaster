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
            List<ViewPergunta> perguntas = null;
            List<ViewResposta> respostas = null;
            List<ViewPerguntaRespota> retorno = null;
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

                respostas = ctx.Respostas.Select(s => new ViewResposta()
                {
                    Id = s.Id,
                    Correta = s.Correta,
                    Resposta = s.Resposta1,
                    IdPergunta = s.IdPergunta
                }).ToList();
            }

            if (perguntas.Count == 0)
            {
                return NotFound();
            }

            retorno = new List<ViewPerguntaRespota>();
            perguntas.ForEach(p =>
            {
                var item = new ViewPerguntaRespota
                {
                    pergunta = p,
                    respostas = respostas.Where(x => x.IdPergunta == p.Id).ToList()
                };
                retorno.Add(item);
            });

            return Ok(retorno);
        }

        public IHttpActionResult Get(int id)
        {
            ViewPergunta pergunta = null;
            List<ViewResposta> respostas = null;
            ViewPerguntaRespota retorno = null;
            using (ctx = new JogoMasterEntities())
            {
                pergunta = ctx.Perguntas
                .Where(x => x.Id == id).Select(s => new ViewPergunta()
                {
                    Id = s.Id,
                    Pergunta = s.Pergunta1,
                    IdNivel = s.IdNivel,
                    IdTema = s.IdTema,
                    Patrocinada = s.Patrocinada
                }).FirstOrDefault();

                if (pergunta == null)
                {
                    return NotFound();
                }

                respostas = ctx.Respostas
                .Where(s => s.IdPergunta == pergunta.Id)
                .Select(s => new ViewResposta()
                {
                    Id = s.Id,
                    Correta = s.Correta,
                    Resposta = s.Resposta1,
                    IdPergunta = s.IdPergunta
                })
                .ToList();

                retorno = new ViewPerguntaRespota
                {
                    pergunta = pergunta,
                    respostas = respostas
                };
            }

            return Ok(retorno);
        }

        public IHttpActionResult Post(ViewPerguntaRespota dados)
        {
            if (dados == null)
            {
                return BadRequest("Dados inválidos.");
            }

            ValidaPergunta(dados.pergunta);
            ValidaResposta(dados.respostas);
            var pergunta_add = new Pergunta
            {
                Pergunta1 = dados.pergunta.Pergunta,
                Patrocinada = dados.pergunta.Patrocinada,
                IdTema = dados.pergunta.IdTema,
                IdNivel = dados.pergunta.IdNivel
            };

            using (ctx = new JogoMasterEntities())
            {
                ctx.Perguntas.Add(pergunta_add);
                ctx.SaveChanges();
            }
            dados.respostas.ForEach(res => res.IdPergunta = pergunta_add.Id);

            using (ctx = new JogoMasterEntities())
            {
                dados.respostas.ForEach(res =>
                {
                    ctx.Respostas.Add(new Resposta()
                    {
                        Correta = res.Correta,
                        IdPergunta = pergunta_add.Id,
                        Resposta1 = res.Resposta,
                    });
                });

                ctx.SaveChanges();
            }

            return Ok("Pergunta cadastrada com sucesso!");
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            List<Resposta> resposta = new List<Resposta>();
            using (ctx = new JogoMasterEntities())
            {
                resposta = ctx.Respostas
                    .Where(x => x.IdPergunta == id)
                    .ToList();

                //ctx.SaveChanges();

                if (resposta == null) { return BadRequest("ID inválido"); }

                resposta.ForEach(res =>
                {
                    ctx.Entry(res).State = System.Data.Entity.EntityState.Deleted;
                    //ctx.SaveChanges();
                });

                var pergunta = ctx.Perguntas
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (pergunta == null) { return BadRequest("ID inválido"); }

                ctx.Entry(pergunta).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
