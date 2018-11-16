using JogoMaster.Models;
using Microsoft.Web.WebSockets;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace JogoMaster.Controllers
{
    public partial class SalaController
    {
        
        public HttpResponseMessage Get(int UsuarioId)
        {
            ValidarUsuario(UsuarioId);

            HttpContext.Current.AcceptWebSocketRequest(new SalaWebSocketHandler(UsuarioId));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class SalaWebSocketHandler : WebSocketHandler
        {
            public static WebSocketCollection salaClients = new WebSocketCollection();
            public JavaScriptSerializer serializer = new JavaScriptSerializer();
            public JogoMasterEntities ctx;
            public SalaPartida SalaPartidaMaster = new SalaPartida();
            private int _usuario;


            public SalaWebSocketHandler(int usuario)
            {
                _usuario = usuario;
            }

            public override void OnOpen()
            {
                salaClients.Add(this);
            }

            public override void OnMessage(string jogador)
            {
                var novoJogador = serializer.Deserialize<CriacaoSala>(jogador);
                novoJogador.UsuarioId = _usuario;
                if (novoJogador.NovaSala)
                {
                    criaNovaSala(novoJogador);
                }
                else
                {
                    using (ctx = new JogoMasterEntities())
                    {
                        SalaPartidaMaster.SalaId = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Id).FirstOrDefault();
                        SalaPartidaMaster.CriadorSala = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Criador).FirstOrDefault();
                        SalaPartidaMaster.Usuarios = ctx.SalasUsuarios.Where(x => x.SalaId == novoJogador.SalaId).Select(x => x.UsuarioId).ToList();
                        SalaPartidaMaster.TotalJogadores = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Jogadores).FirstOrDefault();
                        SalaPartidaMaster.JogadoresNaSala = SalaPartidaMaster.Usuarios.Count;
                        SalaPartidaMaster.Temas = ctx.SalasTemas.Where(x => x.SalaId == novoJogador.SalaId).Select(x => x.TemaId).ToList();
                        SalaPartidaMaster.Nivel = ctx.Salas.Where(x => x.Id == novoJogador.SalaId).Select(x => x.Nivel).FirstOrDefault();
                        SalaPartidaMaster.SalaCheia = SalaPartidaMaster.JogadoresNaSala < SalaPartidaMaster.TotalJogadores ? false : true;
                    }
                }

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
                        Criador = SalaPartidaMaster.CriadorSala,
                        Jogadores = SalaPartidaMaster.TotalJogadores
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
