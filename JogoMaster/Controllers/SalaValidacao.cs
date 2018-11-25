using JogoMaster.Models;
using System.Linq;

namespace JogoMaster.Controllers
{
    public partial class SalaController : Validacao
    {
        private JogoMasterEntities ctx;

        public void ValidarUsuario(int usuario)
        {
            Refute(usuario <= 0, "Usuário inválido");
            using (ctx = new JogoMasterEntities())
            {
                Usuario Usuario = null;
                Usuario = ctx.Usuarios
                    .FirstOrDefault(x => x.Id == usuario);
                Refute(Usuario == null, $"Usuário {usuario} inexistente.");
            }
        }

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

        public void criaNovaSala(CriacaoSala dados, SalaPartida SalaPartidaMaster)
        {
            SalaPartidaMaster.Criador = dados.UsuarioId;
            SalaPartidaMaster.IdNivel = dados.NivelId;
            SalaPartidaMaster.Temas = dados.TemasIds;
            SalaPartidaMaster.MaximoJogadores = dados.Jogadores;

            using (ctx = new JogoMasterEntities())
            {
                var sala = ctx.Salas.Add(new Sala
                {
                    Nivel = SalaPartidaMaster.IdNivel,
                    Criador = SalaPartidaMaster.Criador,
                    Jogadores = SalaPartidaMaster.MaximoJogadores,
                    Ativa = true
                });

                SalaPartidaMaster.Temas.ForEach(temaId =>
                {
                    ctx.SalasTemas.Add(new SalaTemas
                    {
                        SalaId = sala.Id,
                        TemaId = temaId
                    });
                });

                ctx.SaveChanges();
                dados.SalaId = sala.Id;
                SalaPartidaMaster.Id = sala.Id;
            }
        }

        public void adicionaNovoJogador(int salaId, int usuarioId, SalaPartida SalaPartidaMaster)
        {
            SalaPartidaMaster.JogadoresNaSala++;
            SalaPartidaMaster.Jogadores.Add(usuarioId);

            using (ctx = new JogoMasterEntities())
            {
                ctx.SalasUsuarios.Add(new SalaUsuarios
                {
                    SalaId = salaId,
                    UsuarioId = usuarioId
                });
                ctx.SaveChanges();
            }
        }

        public void buscaDadosSala(CriacaoSala novoJogador, SalaPartida SalaPartidaMaster)
        {
            using (ctx = new JogoMasterEntities())
            {
                SalaPartidaMaster.Id = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Id).FirstOrDefault();
                SalaPartidaMaster.Criador = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Criador).FirstOrDefault();
                SalaPartidaMaster.Jogadores = ctx.SalasUsuarios.Where(x => x.SalaId == novoJogador.SalaId).Select(x => x.UsuarioId).ToList();
                SalaPartidaMaster.MaximoJogadores = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Jogadores).FirstOrDefault();
                SalaPartidaMaster.JogadoresNaSala = SalaPartidaMaster.Jogadores.Count;
                SalaPartidaMaster.Temas = ctx.SalasTemas.Where(x => x.SalaId == novoJogador.SalaId).Select(x => x.TemaId).ToList();
                SalaPartidaMaster.IdNivel = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Nivel).FirstOrDefault();
                SalaPartidaMaster.SalaCheia = SalaPartidaMaster.JogadoresNaSala < SalaPartidaMaster.MaximoJogadores ? false : true;
            }
        }

    }
}