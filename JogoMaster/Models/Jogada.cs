using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class Jogada
    {
        public int IdUsuario { get; set; }
        public int IdSala { get; set; }
        public int IdTema { get; set; }
        public bool Finalizou { get; set; }
        public int Pontos { get; set; }
    }

    public class retornoRanking
    {
        public List<ViewRanking> ranking { get; set; }
        public ViewRanking usuario { get; set; }
    }

    public class ViewRanking
    {
        public int Posicao { get; set; }
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string Classificacao { get; set; }
        public int PontosJogada { get; set; }
        public string Skin { get; set; }
    }
}