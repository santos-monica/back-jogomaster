using JogoMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    //[Authorize]
    public partial class AdministradorController
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get()
        {
            IList<ViewAdministrador> administradores = null;

            using (ctx = new JogoMasterEntities())
            {
                administradores = ctx.Administradores.Select(s => new ViewAdministrador()
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Email = s.Email
                }).ToList();
            }

            if (administradores.Count == 0) return NotFound();

            return Ok(administradores);
        }

        public IHttpActionResult Get(int id)
        {
            ViewAdministrador administrador = null;

            using (ctx = new JogoMasterEntities())
            {
                administrador = ctx.Administradores.Where(x => x.Id == id).Select(s => new ViewAdministrador()
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Email = s.Email
                }).FirstOrDefault();
            }

            if (administrador == null) return NotFound();

            return Ok(administrador);
        }

        public IHttpActionResult Post(ViewAdministrador dados)
        {
            if (dados == null) return BadRequest("Dados inválidos.");

            ValidaAdministrador(dados);

            using (ctx = new JogoMasterEntities())
            {
                var adm = new Administrador
                {
                    Nome = dados.Nome,
                    Email = dados.Email,
                    Senha = dados.Senha
                };
                ctx.Administradores.Add(adm);

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ViewAdministrador dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            using (ctx = new JogoMasterEntities())
            {
                var AdministradorAtual = ctx.Administradores
                    .FirstOrDefault(t => t.Id == dados.Id);

                if (AdministradorAtual != null)
                {
                    AdministradorAtual.Nome = dados.Nome;
                    AdministradorAtual.Email = dados.Email;
                    AdministradorAtual.Senha = dados.Senha;
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
                var administrador = ctx.Administradores
                    .FirstOrDefault(x => x.Id == id);

                if (administrador == null) return BadRequest("ID inválido");

                ctx.Entry(administrador).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
