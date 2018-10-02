using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public class TemaController : ApiController
    {
        private JogoMasterEntities ctx = new JogoMasterEntities();

        public IHttpActionResult Get()
        {
            IList<ViewTema> temas = null;

            using (ctx)
            {
                temas = ctx.Temas.Select(s => new ViewTema()
                {
                    Id = s.Id,
                    Tema = s.Tema1
                }).ToList();
            }

            if (temas.Count == 0) return NotFound(); 

            return Ok(temas);
        }

        public IHttpActionResult Get(int id)
        {
            ViewTema tema = null;

            using (ctx)
            {
                tema = ctx.Temas.Where(t => t.Id == id).Select(t => new ViewTema()
                {
                    Id = t.Id,
                    Tema = t.Tema1
                }).FirstOrDefault();
            }

            if (tema == null) return NotFound();

            return Ok(tema);
        }

        public IHttpActionResult Post(ViewTema tema)
        {
            if (tema == null)
                return BadRequest("Dados inválidos.");

            using (ctx)
            {
                ctx.Temas.Add(new Tema()
                {
                    Tema1 = tema.Tema
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewTema tema)
        {
            if (tema == null)
                return BadRequest("Dados inválidos.");

            using (ctx)
            {
                var TemaAtual = ctx.Temas.Where(t => t.Id == tema.Id)
                                                        .FirstOrDefault<Tema>();

                if (TemaAtual != null)
                {
                    TemaAtual.Tema1 = tema.Tema;
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

            using (ctx)
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
