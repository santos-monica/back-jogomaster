using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JogoMaster.Controllers
{
    //[Authorize]
    [EnableCors(origins:"*", headers: "*", methods: "*")]
    public partial class TemaController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewTema> temas = null;

            using (ctx = new JogoMasterEntities())
            {
                temas = ctx.Temas.Select(s => new ViewTema()
                {
                    Id = s.Id,
                    Tema = s.Tema1,
                    Icone = s.Icone,
                    Cor = s.Cor
                }).ToList();
            }

            if (temas.Count == 0) return NotFound();

            return Ok(temas);
        }

        public IHttpActionResult Get(int id)
        {
            ViewTema tema = null;

            using (ctx = new JogoMasterEntities())
            {
                tema = ctx.Temas.Where(t => t.Id == id).Select(t => new ViewTema()
                {
                    Id = t.Id,
                    Tema = t.Tema1,
                    Icone = t.Icone,
                    Cor = t.Cor
                }).FirstOrDefault();
            }

            if (tema == null) return NotFound();

            return Ok(tema);
        }

        [Route("api/buscarTemas")]
        public IHttpActionResult Post(ListaIds lista)
        {
            List<ViewTema> temas = new List<ViewTema>();

            using (ctx = new JogoMasterEntities())
            {
                lista.ids.ForEach(id =>
                {
                    var tema = ctx.Temas.Where(t => t.Id == id).Select(t => new ViewTema
                    {
                        Id = t.Id,
                        Tema = t.Tema1,
                        Cor = t.Cor,
                        Icone = t.Icone
                    }
                    ).FirstOrDefault();

                    temas.Add(tema);
                });
            }

            return Ok(temas);
        }

        public IHttpActionResult Post(ViewTema dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            ValidaTema(dados);

            using (ctx = new JogoMasterEntities())
            {
                ctx.Temas.Add(new Tema()
                {
                    Tema1 = dados.Tema,
                    Icone = dados.Icone,
                    Cor = dados.Cor
                });

                ctx.SaveChanges();
            }

            return Ok(dados);
        }

        public IHttpActionResult Put(ViewTema dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var TemaAtual = ctx.Temas.Where(t => t.Id == dados.Id)
                                                        .FirstOrDefault<Tema>();

                if (TemaAtual != null)
                {
                    TemaAtual.Tema1 = dados.Tema;
                    TemaAtual.Icone = dados.Icone;
                    TemaAtual.Cor = dados.Cor;
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
            {
                return BadRequest("ID inválido.");
            }

            using (ctx = new JogoMasterEntities())
            {
                var tema = ctx.Temas
                    .Where(t => t.Id == id)
                    .FirstOrDefault();

                if (tema == null) return BadRequest("ID inválido");
                
                ctx.Entry(tema).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
