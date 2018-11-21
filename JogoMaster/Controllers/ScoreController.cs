using JogoMaster.Models;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    [Authorize]
    public class ScoreController : ApiController
    {
        public HttpResponseMessage Get(string Username)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ScoreWebSocketHandler(Username));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class ScoreWebSocketHandler : WebSocketHandler
        {
            private static WebSocketCollection _scoreClients = new WebSocketCollection();
            private string _username;

            public ScoreWebSocketHandler(string username)
            {
                _username = username;
            }

            public override void OnOpen()
            {
                _scoreClients.Add(this);
            }

            public override void OnMessage(string message)
            {
                _scoreClients.Broadcast(message);
            }
        }
    }
}
