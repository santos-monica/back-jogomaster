using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JogoMaster
{
    public class MasterMessageRelay
    {
        public MasterMessageRelay(IHubContext<MasterHub> hubContext)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {

                }
            });
        }
    }
}