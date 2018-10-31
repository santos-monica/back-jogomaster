using JogoMaster.Models;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class NivelController : Validacao
    {
        private void ValidaNivel(ViewNivel dados)
        {
            Refute(string.IsNullOrEmpty(dados.Nivel), "Informe o Nível");
            Nivel nivel = null;
            using(ctx = new JogoMasterEntities())
            {
                nivel = ctx.Niveis.FirstOrDefault(x => x.Nivel1.ToLower() == dados.Nivel.ToLower());
                Refute(nivel != null, "Nível existente.");
            }
        }
    }
}