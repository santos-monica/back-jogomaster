using JogoMaster.Models;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class NivelController : Validacao
    {
        private void ValidaNivel(ViewNivel dados)
        {
            Refute(string.IsNullOrEmpty(dados.Nivel), "Informe o Nível");
            Refute(dados.Pontos <= 0, "Informe os Pontos");
            Nivel nivel = null;
            using(ctx = new JogoMasterEntities())
            {
                nivel = ctx.Niveis.FirstOrDefault(x => x.Nivel1.ToLower() == dados.Nivel.ToLower());
                Refute(nivel != null, "Nível existente.");

                nivel = ctx.Niveis.FirstOrDefault(x => x.Pontos == dados.Pontos);
                Refute(nivel != null, "Já existe um nível com esta pontuação.");
            }
        }
    }
}