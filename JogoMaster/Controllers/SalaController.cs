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
                    var temasSala = ctx.SalasTemas.Where(s => s.SalaId == sala.Id).Select(s => s.TemaId).ToList();

                    var teste = from sa in ctx.SalasUsuarios
                                join u in ctx.Usuarios on sa.UsuarioId equals u.Id
                                where sa.SalaId == sala.Id
                                select u.Id;

                    salas.Add(new ViewSala
                    {
                        Criador = ctx.Usuarios.Where(x => x.Id == sala.Criador).Select(x => x.Id).FirstOrDefault(),
                        Jogadores = teste.ToList(),
                        Id = sala.Id,
                        Temas = temasSala,
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

                var novoJogador = serializer.Deserialize<CriacaoSala>(jogador);
                novoJogador.UsuarioId = _usuario;
                var helper = new SalaController();
                var erros = new List<string>();
                helper.ValidaDadosSala(novoJogador, erros);
                if (erros.Any())
                {
                    var retorno = $"{{ \"erro\": \"{erros[0]}\", \"deuErro\": true}}";
                    salaClients.Broadcast(retorno);
                    return;
                }
                if (!helper.JogadorEmNenhumaSala(novoJogador.UsuarioId))
                {
                    salaClients.Broadcast("{ \"erro\": \"Jogador já está em uma sala\", \"deuErro\": true}");
                    return;
                };

                if (novoJogador.NovaSala)
                {
                    helper.criaNovaSala(novoJogador, SalaPartidaMaster);
                }
                else
                {
                    helper.buscaDadosSala(novoJogador, SalaPartidaMaster);
                }

                helper.adicionaNovoJogador(novoJogador.SalaId, novoJogador.UsuarioId, SalaPartidaMaster);

                if (SalaPartidaMaster.JogadoresNaSala == SalaPartidaMaster.MaximoJogadores)
                {
                    SalaPartidaMaster.SalaCheia = true;
                    using (ctx = new JogoMasterEntities())
                    {
                        var sala = new Sala() { Id = SalaPartidaMaster.Id, Ativa = false };
                        ctx.Salas.Attach(sala);
                        ctx.Entry(sala).Property(x => x.Ativa).IsModified = true;
                        ctx.SaveChanges();
                    };

                }
                var salaAtualizada = serializer.Serialize(SalaPartidaMaster);
                salaClients.Broadcast(salaAtualizada);
            }
        }

    }
}
