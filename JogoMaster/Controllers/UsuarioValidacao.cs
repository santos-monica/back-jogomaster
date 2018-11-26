using JogoMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Controllers
{
    public partial class UsuarioController : Validacao
    {
        private void ValidaUsuarioPadrao(ViewUsuario user)
        {
            var rnd = new Random(DateTime.Now.Millisecond);

            user.Username = "user" + rnd.Next();
            user.Nome = user.Username;
            user.IdClassificacao = 1;
            user.Pontos = 0;
            user.Senha = "senha";
            user.Skin = "default.png";
            user.Email = "none";

        }
        private void ValidaNovoUsuario(ViewUsuario user)
        {
            Usuario usuario = null;
            using (ctx = new JogoMasterEntities())
            {
                usuario = ctx.Usuarios.Where(u => u.Username == user.Username).FirstOrDefault();
                Refute(usuario != null, "Username já cadastrado.");

                usuario = ctx.Usuarios.Where(u => u.Email == user.Email).FirstOrDefault();
                Refute(usuario != null, "Email já cadastrado.");
            }
        }
    }
}