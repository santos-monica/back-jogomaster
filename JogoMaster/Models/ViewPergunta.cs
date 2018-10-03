using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewPergunta
    {
        public int Id { get; set; }
        public string Pergunta { get; set; }
        public bool Patrocinada { get; set; }
        public int IdTema { get; set; }
        public int IdNivel { get; set; }
    }
}