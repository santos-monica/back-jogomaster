using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    //[Authorize]
    public partial class NivelController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewNivel> niveis = null;

            using (ctx = new JogoMasterEntities())
            {
                niveis = ctx.Niveis.Select(s => new ViewNivel()
                {
                    Id = s.Id,
                    Nivel = s.Nivel1
                }).ToList();
            }

            if (niveis.Count == 0) return NotFound();

            return Ok(niveis);
        }

        public IHttpActionResult Get(int id)
        {
            ViewNivel nivel = null;

            using (ctx = new JogoMasterEntities())
            {
                nivel = ctx.Niveis.Where(x => x.Id == id).Select(s => new ViewNivel()
                {
                    Id = s.Id,
                    Nivel = s.Nivel1
                }).FirstOrDefault();
            }

            if (nivel == null) return NotFound();

            return Ok(nivel);
        }

        public IHttpActionResult Post(ViewNivel dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            ValidaNivel(dados);

            using (ctx = new JogoMasterEntities())
            {
                ctx.Niveis.Add(new Nivel()
                {
                    Nivel1 = dados.Nivel
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewNivel dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var NivelAtual = ctx.Niveis.Where(t => t.Id == dados.Id)
                                                        .FirstOrDefault<Nivel>();

                if (NivelAtual != null)
                {
                    NivelAtual.Nivel1 = dados.Nivel;
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
                var usuario = ctx.Niveis
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (usuario == null) return BadRequest("ID inválido");

                ctx.Entry(usuario).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
