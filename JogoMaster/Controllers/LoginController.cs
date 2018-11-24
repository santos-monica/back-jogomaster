using JogoMaster.Models;
using JogoMaster.Util;
using System.Linq;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public class LoginController : Validacao
    {
        private JogoMasterEntities ctx;

        public IHttpActionResult Post(ViewLogin dados)
        {
            Usuario usuario = null;
            
            using (ctx = new JogoMasterEntities())
            {
                usuario = ctx.Usuarios
                    .Where(user => user.Username == dados.Username && user.Senha == dados.Senha)
                    .FirstOrDefault();
            }

            if (usuario == null) return BadRequest("Login inválido.");

            return Ok(usuario.Id);
        }
    }
}
