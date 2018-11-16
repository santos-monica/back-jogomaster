using JogoMaster.Models;
using Microsoft.Web.WebSockets;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace JogoMaster.Controllers
{
    public partial class SalaController
    {
        public int SalaCriada { get; set; }

        public HttpResponseMessage Get(int UsuarioId, int Tema1, int Tema2, int Tema3, int Tema4, int Tema5, int Nivel, int Jogadores)
        {
            var dadosNovaSala = new CriacaoSala
            {
                Jogadores = Jogadores,
                NivelId = Nivel,
                UsuarioId = UsuarioId,
                TemasIds = new List<int> { Tema1, Tema2, Tema3, Tema4, Tema5 },
                NovaSala = true
            };

            ValidaDadosSala(dadosNovaSala);

            var socketHandler = new SalaWebSocketHandler(dadosNovaSala);
            HttpContext.Current.AcceptWebSocketRequest(socketHandler);
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        public HttpResponseMessage Get(int UsuarioId, int SalaId)
        {
            var dadosEntrarSala = new CriacaoSala
            {
                UsuarioId = UsuarioId,
                SalaId = SalaId,
                NovaSala = false
            };

            ValidaDadosSala(dadosEntrarSala);

            HttpContext.Current.AcceptWebSocketRequest(new SalaWebSocketHandler(dadosEntrarSala));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class SalaWebSocketHandler : WebSocketHandler
        {
            public static WebSocketCollection salaClients = new WebSocketCollection();
            public SalaPartida SalaPartidaMaster = new SalaPartida();
            public JogoMasterEntities ctx;
            public JavaScriptSerializer serializer = new JavaScriptSerializer();

            public SalaWebSocketHandler(CriacaoSala dadosSala)
            {
                if (dadosSala.NovaSala)
                {
                    criaNovaSala(dadosSala);
                }

                adicionaNovoJogador(dadosSala.UsuarioId, 1);
            }

            public override void OnOpen()
            {
                salaClients.Add(this);
            }

            public override void OnMessage(string jogador)
            {
                var novoJogador = serializer.Deserialize<ParticiparSala>(jogador);

                adicionaNovoJogador(novoJogador.SalaId, novoJogador.UsuarioId);

                if (SalaPartidaMaster.JogadoresNaSala == SalaPartidaMaster.TotalJogadores)
                {
                    SalaPartidaMaster.SalaCheia = true;
                }

                var salaAtualizada = serializer.Serialize(SalaPartidaMaster);
                salaClients.Broadcast(salaAtualizada);
            }

            // Criação de sala e inserção de usuário
            private void criaNovaSala(CriacaoSala dados)
            {
                SalaPartidaMaster.CriadorSala = dados.UsuarioId;
                SalaPartidaMaster.Nivel = dados.NivelId;
                SalaPartidaMaster.Temas = dados.TemasIds;
                SalaPartidaMaster.TotalJogadores = dados.Jogadores;

                using (ctx = new JogoMasterEntities())
                {
                    var sala = ctx.Salas.Add(new Sala
                    {
                        Nivel = SalaPartidaMaster.Nivel,
                        Criador = SalaPartidaMaster.CriadorSala

                    });

                    SalaPartidaMaster.Temas.ForEach(temaId =>
                    {
                        ctx.SalasTemas.Add(new SalaTemas
                        {
                            SalaId = sala.Id,
                            TemaId = temaId
                        });
                    });

                    ctx.SalasUsuarios.Add(new SalaUsuarios
                    {
                        SalaId = sala.Id,
                        UsuarioId = dados.UsuarioId
                    });

                    ctx.SaveChanges();
                    SalaPartidaMaster.SalaId = sala.Id;
                }
            }

            private void adicionaNovoJogador(int salaId, int usuarioId)
            {
                SalaPartidaMaster.JogadoresNaSala++;
                SalaPartidaMaster.Usuarios.Add(usuarioId);

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
        }
    }
}
