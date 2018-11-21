using JogoMaster.Models;
using JogoMaster.Util;
using System.Collections.Generic;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class AdministradorController : Validacao
    {
        private void ValidaAdministrador(ViewAdministrador dados)
        {
            Refute(string.IsNullOrEmpty(dados.Nome), "Informe o Nome.");
            Refute(string.IsNullOrEmpty(dados.Senha), "Informe a Senha.");
            Refute(string.IsNullOrEmpty(dados.Email), "Informe o endereço de e-mail.");
            Refute(!IsValidEmail(dados.Email), "Email inválido.");
            Administrador administrador = null;
            using (ctx = new JogoMasterEntities())
            {
                administrador = ctx.Administradores
                    .FirstOrDefault(x => x.Email == dados.Email);
            }
            Refute(administrador != null, "E-mail já cadastrado.");
            dados.Senha = Criptografia.MD5Hash(dados.Senha);
        }
    }
}