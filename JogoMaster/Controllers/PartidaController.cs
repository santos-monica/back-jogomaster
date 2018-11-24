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
    public class PartidaController : ApiController
    {
        public HttpResponseMessage Get(int UsuarioId)
        {
            HttpContext.Current.AcceptWebSocketRequest(new PartidaWebSocketHandler(UsuarioId));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        public class PartidaWebSocketHandler : WebSocketHandler
        {
            public static WebSocketCollection partidaClients = new WebSocketCollection();
            public JavaScriptSerializer serializer = new JavaScriptSerializer();
            public JogoMasterEntities ctx;
            private int _usuario;

            public PartidaWebSocketHandler(int usuario)
            {
                _usuario = usuario;
            }

            public override void OnOpen()
            {
                partidaClients.Add(this);
            }

            public override void OnMessage(string jogada)
            {
                var novaJogada = serializer.Deserialize<Jogada>(jogada);
                novaJogada.IdUsuario = _usuario;
                var retorno = serializer.Serialize(novaJogada);
                partidaClients.Broadcast(retorno);
            }
        }
    }
}
