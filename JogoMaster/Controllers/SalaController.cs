using JogoMaster.Models;
using Microsoft.Web.WebSockets;
using System;
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

        public IHttpActionResult Get()
        {
            var salas = new List<ViewSala>();

            using (ctx = new JogoMasterEntities())
            {
                var bancosalas = ctx.Salas
                    .Where(s => s.Ativa == true)
                    .ToList();

                bancosalas.ForEach(sala =>
                {
                    var jogadoresSala = ctx.SalasUsuarios.Where(s => s.SalaId == sala.Id).Select(s => s.UsuarioId).ToList();

                    var teste = from sa in ctx.SalasUsuarios
                                join u in ctx.Usuarios on sa.UsuarioId equals u.Id
                                where sa.SalaId == sala.Id
                                select u.Username;

                    salas.Add(new ViewSala
                    {
                        Criador = ctx.Usuarios.Where(x => x.Id == sala.Criador).Select(x => x.Username).FirstOrDefault(),
                        Jogadores = teste.ToList(),
                        Id = sala.Id,
                        IdNivel = sala.Nivel,
                        JogadoresNaSala = teste.Count(),
                        MaximoJogadores = sala.Jogadores
                    });
                });
            }

            if (salas.Count == 0) return NotFound();

            return Ok(salas);
        }



        public class SalaWebSocketHandler : WebSocketHandler
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
                if (jogador == "getsalas")
                {
                    var salas = new List<ViewSala>();

                    using (ctx = new JogoMasterEntities())
                    {
                        var bancosalas = ctx.Salas
                            .Where(s => s.Ativa == true)
                            .ToList();

                        bancosalas.ForEach(sala =>
                        {
                            var jogadoresSala = ctx.SalasUsuarios.Where(s => s.SalaId == sala.Id).Select(s => s.UsuarioId).ToList();

                            var teste = from sa in ctx.SalasUsuarios
                                        join u in ctx.Usuarios on sa.UsuarioId equals u.Id
                                        where sa.SalaId == sala.Id
                                        select u.Username;

                            salas.Add(new ViewSala
                            {
                                Criador = ctx.Usuarios.Where(x => x.Id == sala.Criador).Select(x => x.Username).FirstOrDefault(),
                                Jogadores = teste.ToList(),
                                Id = sala.Id,
                                IdNivel = sala.Nivel,
                                JogadoresNaSala = teste.Count(),
                                MaximoJogadores = sala.Jogadores
                            });
                        });
                    }

                    if (salas.Count == 0) return;

                    var retorno = serializer.Serialize(salas);
                    salaClients.Broadcast(retorno);
                }
                else
                {
                    var novoJogador = serializer.Deserialize<CriacaoSala>(jogador);
                    novoJogador.UsuarioId = _usuario;
                    var helper = new SalaController();
                    helper.ValidaDadosSala(novoJogador);

                    if (novoJogador.NovaSala)
                    {
                        helper.criaNovaSala(novoJogador, SalaPartidaMaster);
                    }
                    else
                    {
                        helper.buscaDadosSala(novoJogador, SalaPartidaMaster);
                    }

                    helper.adicionaNovoJogador(novoJogador.SalaId, novoJogador.UsuarioId, SalaPartidaMaster);
             
                    if (SalaPartidaMaster.JogadoresNaSala == SalaPartidaMaster.TotalJogadores)
                    {
                        SalaPartidaMaster.SalaCheia = true;
                    }

                    var salaAtualizada = serializer.Serialize(SalaPartidaMaster);
                    salaClients.Broadcast(salaAtualizada);
                }
            }
        }
    }
}
