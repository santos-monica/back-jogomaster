using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public class UsuarioController : ApiController
    {
        private JogoMasterEntities ctx = new JogoMasterEntities();

        public IHttpActionResult Get()
        {
            IList<ViewUsuario> usuarios = null;

            using (ctx)
            {
                usuarios = ctx.Usuarios.Select(s => new ViewUsuario()
                {
                    Id = s.Id,
                    Email = s.Email,
                    Nome = s.Nome,
                    Pontos = s.Pontos,
                    Senha = s.Senha
                }).ToList();
            }

            if (usuarios.Count == 0) return NotFound(); 

            return Ok(usuarios);
        }

        public IHttpActionResult Get(int id)
        {
            ViewUsuario usuario = null;

            using (ctx)
            {
                usuario = ctx.Usuarios.Where(x => x.Id == id).Select(s => new ViewUsuario()
                {
                    Id = s.Id,
                    Email = s.Email,
                    Nome = s.Nome,
                    Pontos = s.Pontos,
                    Senha = s.Senha
                }).FirstOrDefault();
            }

            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        public IHttpActionResult Post(ViewUsuario dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx)
            {
                ctx.Usuarios.Add(new Usuario()
                {
                    Email = dados.Email,
                    Nome = dados.Nome,
                    Pontos = dados.Pontos,
                    Senha = dados.Senha,
                    IdClassificacao = dados.IdClassificacao

                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewUsuario usuario)
        {
            if (usuario == null)
                return BadRequest("Dados inválidos.");

            using (ctx)
            {
                var UsuarioAtual = ctx.Usuarios.Where(t => t.Id == usuario.Id)
                                                        .FirstOrDefault<Usuario>();

                if (UsuarioAtual != null)
                {
                    UsuarioAtual.Nome = usuario.Nome;
                    UsuarioAtual.Pontos = usuario.Pontos;
                    UsuarioAtual.Senha = usuario.Senha;
                    UsuarioAtual.IdClassificacao = usuario.IdClassificacao;
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
                var usuario = ctx.Usuarios
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
