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
                    Skin = s.Skin,
                    Username = s.Username,
                    IdClassificacao = s.IdClassificacao,
                    Classificacao = ctx.Classificacoes.Where(c => c.Id == s.IdClassificacao).FirstOrDefault().Classificacao1,
                    Pontos = s.Pontos,
                    Cadastrado = s.Cadastrado
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
                    Skin = s.Skin,
                    Username = s.Username,
                    IdClassificacao = s.IdClassificacao,
                    Classificacao = ctx.Classificacoes.Where(c => c.Id == s.IdClassificacao).FirstOrDefault().Classificacao1,
                    Pontos = s.Pontos,
                    Cadastrado = s.Cadastrado
                }).FirstOrDefault();


            }

            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        [AllowAnonymous]
        public IHttpActionResult Post(ViewUsuario dados)
        {
            if (dados == null)
                return BadRequest("Dados inválidos.");

            if(dados.Cadastrado == false)
            {
                ValidaUsuarioPadrao(dados);
            } else
            {
                ValidaNovoUsuario(dados);
            }

            var usuario = new Usuario
            {
                Nome = dados.Nome,
                Username = dados.Username,
                Email = dados.Email,
                Senha = dados.Senha,
                Pontos = 0,
                Cadastrado = dados.Cadastrado,
                Skin = dados.Skin,
                IdClassificacao = 1,   
            };

            using (ctx = new JogoMasterEntities())
            {
                ctx.Usuarios.Add(usuario);
                ctx.SaveChanges();
            }

            return Ok(usuario.Id);
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
                    UsuarioAtual.Username = usuario.Username;
                    UsuarioAtual.Skin = usuario.Skin;
                    UsuarioAtual.Pontos = usuario.Pontos;
                    UsuarioAtual.Cadastrado = usuario.Cadastrado;
                    UsuarioAtual.IdClassificacao = ctx.Classificacoes.Where(c => c.PontuacaoMinima <= usuario.Pontos && c.PontuacaoMaxima >= usuario.Pontos).FirstOrDefault().Id;
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
