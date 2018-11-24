using JogoMaster.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    //[Authorize]
    public partial class UsuarioController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewUsuario> usuarios = null;

            using (ctx = new JogoMasterEntities())
            {
                usuarios = ctx.Usuarios.Select(s => new ViewUsuario()
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Email = s.Email,
                    Senha = s.Senha,
                    Pontos = s.Pontos
                }).ToList();
            }

            if (usuarios.Count == 0) return NotFound(); 

            return Ok(usuarios);
        }

        public IHttpActionResult Get(int id)
        {
            ViewUsuario usuario = null;

            using (ctx = new JogoMasterEntities())
            {
                usuario = ctx.Usuarios.Where(x => x.Id == id).Select(s => new ViewUsuario()
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Email = s.Email,
                    Senha = s.Senha,
                    Pontos = s.Pontos
                }).FirstOrDefault();
            }

            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        [AllowAnonymous]
        public IHttpActionResult Post(ViewUsuario dados)
        {

            //return BadRequest("errrrooou");
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                ctx.Usuarios.Add(new Usuario()
                {
                    Nome = dados.Nome,
                    Username = dados.Username,
                    Email = dados.Email,
                    Senha = dados.Senha,
                    Pontos = 0,
                    IdClassificacao = 1
                });
                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewUsuario usuario)
        {
            if (usuario == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var UsuarioAtual = ctx.Usuarios.Where(t => t.Id == usuario.Id)
                                                        .FirstOrDefault<Usuario>();

                if (UsuarioAtual != null)
                {
                    UsuarioAtual.Nome = usuario.Nome;
                    UsuarioAtual.Email = usuario.Email;
                    UsuarioAtual.Senha = usuario.Senha;
                    UsuarioAtual.Pontos = usuario.Pontos;
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

            using (ctx = new JogoMasterEntities())
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
