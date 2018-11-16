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
    public class SalaController : ApiController
    {
        public HttpResponseMessage Get(int UsuarioId, string Username, int Tema, int Nivel, int Jogadores)
        {
            var dados = new CriacaoSala
            {
                Jogadores = Jogadores,
                NivelId = Nivel,
                Username = Username,
                UsuarioId = UsuarioId,
                TemasIds = new List<int> { Tema },
                NovaSala = true
            };

            HttpContext.Current.AcceptWebSocketRequest(new SalaWebSocketHandler(dados));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        public HttpResponseMessage Get(int UsuarioId, string Username)
        {
            var dados = new CriacaoSala
            {
                Username = Username,
                UsuarioId = UsuarioId,
                NovaSala = false
            };

            HttpContext.Current.AcceptWebSocketRequest(new SalaWebSocketHandler(dados));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class SalaWebSocketHandler : WebSocketHandler
        {
            private static WebSocketCollection _salaClients = new WebSocketCollection();
            private List<int> UsuarioIds = new List<int>();
            private List<string> Usernames = new List<string>();
            private int NivelId;
            private List<int> TemasIds = new List<int>();
            private int MaximoJogadores;
            private int JogadoresNaSala = 0;
            private int UsuarioCriador;

            private Sala SalaPartida = new Sala();

            public SalaWebSocketHandler(CriacaoSala dados)
            {
                if (dados.NovaSala)
                {
                    UsuarioCriador = dados.UsuarioId;
                    UsuarioIds.Add(dados.UsuarioId);
                    Usernames.Add(dados.Username);
                    NivelId = dados.NivelId;
                    TemasIds = dados.TemasIds;
                    MaximoJogadores = dados.Jogadores;
                    JogadoresNaSala++;

                    SalaPartida.Nivel = dados.NivelId;
                    SalaPartida.Temas = dados.TemasIds;
                    SalaPartida.TotalJogadores = dados.Jogadores;
                }
                else
                {
                    JogadoresNaSala++;
                    Usernames.Add(dados.Username);
                    UsuarioIds.Add(dados.UsuarioId);
                }

                SalaPartida.JogadoresNaSala = JogadoresNaSala;
                SalaPartida.Usernames = Usernames;
                SalaPartida.Usuarios = UsuarioIds;
            }

            public override void OnOpen()
            {
                _salaClients.Add(this);
            }

            public override void OnMessage(string teste)
            {
                var serializer = new JavaScriptSerializer();
                var novoJogador = serializer.Deserialize<ParticiparSala>(teste);

                SalaPartida.Usernames.Add(novoJogador.Username);
                SalaPartida.Usuarios.Add(novoJogador.UsuarioId);
                SalaPartida.JogadoresNaSala++;

                if (SalaPartida.JogadoresNaSala == SalaPartida.TotalJogadores)
                {
                    SalaPartida.SalaCheia = true;
                }

                var salaAtualizada = serializer.Serialize(SalaPartida);
                _salaClients.Broadcast(salaAtualizada);
            }
        }
    }
}
