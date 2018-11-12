using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class Jogada
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public int IdTema { get; set; }
        public bool Correta { get; set; }
    }
}