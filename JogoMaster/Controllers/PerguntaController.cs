using JogoMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    [Authorize]
    public partial class PerguntaController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            List<ViewPergunta> perguntas = null;
            List<ViewResposta> respostas = null;
            List<ViewPerguntaResposta> retorno = null;
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

            retorno = new List<ViewPerguntaResposta>();
            perguntas.ForEach(p =>
            {
                var item = new ViewPerguntaResposta
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
            ViewPerguntaResposta retorno = null;
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

                retorno = new ViewPerguntaResposta
                {
                    pergunta = pergunta,
                    respostas = respostas
                };
            }

            return Ok(retorno);
        }

        [Route("api/buscarPerguntas")]
        public IHttpActionResult Post(ConsultaNivelTemas dados)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var perguntas = new List<ViewPergunta>();
            var retorno = new ListViewPerguntaResposta();

            ValidaDadosConsulta(dados);

            using (ctx = new JogoMasterEntities())
            {
                dados.idsTema.ForEach(tema =>
                {
                    var perguntasBanco = ctx.Perguntas
                    .Where(x => x.IdNivel == dados.idNivel && x.IdTema == tema)
                    .Select(x => new ViewPergunta()
                    {
                        Id = x.Id,
                        Pergunta = x.Pergunta1,
                        IdNivel = x.IdNivel,
                        IdTema = x.IdTema,
                        Patrocinada = x.Patrocinada
                    }).ToList();

                    if (perguntasBanco.Any())
                    {
                        perguntas = perguntasBanco.OrderBy(x => rnd.Next()).Take(4).ToList();

                        perguntas.ForEach(pergunta =>
                        {
                            var res = ctx.Respostas
                            .Where(x => x.IdPergunta == pergunta.Id)
                            .Select(x => new ViewResposta()
                            {
                                Id = x.Id,
                                Correta = x.Correta,
                                Resposta = x.Resposta1,
                                IdPergunta = x.IdPergunta
                            })
                            .ToList();

                            var perRes = new ViewPerguntaResposta
                            {
                                pergunta = pergunta,
                                respostas = res
                            };

                            retorno.lista.Add(perRes);
                        });
                    }
                });
            }
            return Ok(retorno.lista);
        }

        public IHttpActionResult Post(ListViewPerguntaResposta dados)
        {
            if (dados == null || !dados.lista.Any())
            {
                return BadRequest("Dados inválidos.");
            }

            dados.lista.ForEach(item =>
            {
                ValidaPergunta(item.pergunta);
                ValidaResposta(item.respostas);

                var pergunta_add = new Pergunta
                {
                    Pergunta1 = item.pergunta.Pergunta,
                    Patrocinada = item.pergunta.Patrocinada,
                    IdTema = item.pergunta.IdTema,
                    IdNivel = item.pergunta.IdNivel
                };

                using (ctx = new JogoMasterEntities())
                {
                    ctx.Perguntas.Add(pergunta_add);
                    ctx.SaveChanges();

                    item.respostas.ForEach(res => res.IdPergunta = pergunta_add.Id);

                    item.respostas.ForEach(res =>
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
            });

            return Ok("Perguntas cadastradas com sucesso!");
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

                if (resposta == null) { return BadRequest("ID inválido"); }

                resposta.ForEach(res =>
                {
                    ctx.Entry(res).State = System.Data.Entity.EntityState.Deleted;
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
