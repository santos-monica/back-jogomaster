using JogoMaster.Models;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class SalaController : Validacao
    {
        private JogoMasterEntities ctx;

        public void ValidaDadosSala(CriacaoSala dados)
        {
            Refute(dados.UsuarioId <= 0, "Usuário inválido");
            
            using (ctx = new JogoMasterEntities())
            {
                if (dados.NovaSala)
                {
                    Refute(dados.NivelId <= 0, "Nível inválido");
                    Refute(dados.TemasIds.Count != 5, "Quantidade de temas inválida");
                    Refute(dados.Jogadores < 2 || dados.Jogadores > 4, "Quantidade de jogadores inválida");
                    dados.TemasIds.ForEach(tema =>
                    {
                        Tema Tema = null;
                        Tema = ctx.Temas
                            .FirstOrDefault(x => x.Id == tema);
                        Refute(Tema == null, $"Tema {tema} inexistente.");
                    });

                    Nivel Nivel = null;
                    Nivel = ctx.Niveis
                        .FirstOrDefault(x => x.Id == dados.NivelId);
                    Refute(Nivel == null, $"Nível {dados.NivelId} inexistente.");
                }
                else
                {
                    Refute(dados.SalaId <= 0, "Sala inválida");
                    Sala Sala = null;
                    Sala = ctx.Salas
                        .FirstOrDefault(x => x.Id == dados.SalaId);
                    Refute(Sala == null, $"Sala {dados.SalaId} inexistente.");
                }

                Usuario Usuario = null;
                Usuario = ctx.Usuarios
                    .FirstOrDefault(x => x.Id == dados.UsuarioId);
                Refute(Usuario == null, $"Usuário {dados.UsuarioId} inexistente.");
            }
        }
    }
}