using JogoMaster.Util;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public class LoginAdminController : Validacao
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Get(string Email, string Senha)
        {
            Administrador administradores = null;
            var md5Senha = Criptografia.MD5Hash(Senha);

            using (ctx = new JogoMasterEntities())
            {
                administradores = ctx.Administradores
                    .FirstOrDefault(x => x.Email == Email && x.Senha == md5Senha);
            }

            if (administradores == null) return BadRequest("Login inválido.");

            return Ok("Login válido.");
        }
    }
}
