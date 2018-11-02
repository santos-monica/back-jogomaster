using JogoMaster.Models;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class TemaController : Validacao
    {
        private void ValidaTema(ViewTema dados)
        {
            Refute(string.IsNullOrEmpty(dados.Tema), "Inválido");
            Refute(string.IsNullOrEmpty(dados.Cor), "Inválido");
            Refute(string.IsNullOrEmpty(dados.Icone), "Inválido");
            using (ctx = new JogoMasterEntities())
            {
                Tema tema = null;
                tema = ctx.Temas
                    .FirstOrDefault(x => x.Tema1.ToLower() == dados.Tema.ToLower());
                Refute(tema != null, "Tema já cadastrado.");
            }
        }
    }
}