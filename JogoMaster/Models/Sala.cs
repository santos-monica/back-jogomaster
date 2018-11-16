using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class CriacaoSala
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; }
        public int NivelId { get; set; }
        public List<int> TemasIds { get; set; }
        public int Jogadores { get; set; }
        public bool NovaSala { get; set; }

        public CriacaoSala()
        {
            TemasIds = new List<int>();
        }
    }

    public class ParticiparSala
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; }
    }

    public class Sala
    {
        public int SalaId { get; set; }
        public int CriadorSala { get; set; }
        public List<int> Usuarios { get; set; }
        public List<string> Usernames { get; set; }
        public int TotalJogadores { get; set; }
        public int JogadoresNaSala { get; set; }
        public List<int> Temas { get; set; }
        public int Nivel { get; set; }
        public bool SalaCheia { get; set; }

        public Sala()
        {
            Usuarios = new List<int>();
            Usernames = new List<string>();
            Temas = new List<int>();
            SalaCheia = false;
            JogadoresNaSala = 0;
        }
    }

}