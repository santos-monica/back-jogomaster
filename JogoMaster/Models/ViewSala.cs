using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewSala
    {
        public int Id { get; set; }
        public string Criador { get; set; }
        public List<string> Jogadores { get; set; }
        public int JogadoresNaSala { get; set; }
        public int MaximoJogadores { get; set; }
    }
}