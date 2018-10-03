using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewResposta
    {
        public int Id { get; set; }
        public bool Correta { get; set; }
        public string Resposta { get; set; }
        public int IdPergunta { get; set; }
    }
}