using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewClassificacao
    {
        public int Id { get; set; }
        public string Classificacao { get; set; }
        public int PontuacaoMinima { get; set; }
        public int PontuacaoMaxima { get; set; }
    }
}   