﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewSala
    {
        public int Id { get; set; }
        public string Criador { get; set; }
        public int IdNivel { get; set; }
        public List<string> Jogadores { get; set; }
        public int JogadoresNaSala { get; set; }
        public int MaximoJogadores { get; set; }
    }

    public class CriacaoSala
    {
        public int UsuarioId { get; set; }
        public int NivelId { get; set; }
        public List<int> TemasIds { get; set; }
        public int Jogadores { get; set; }
        public bool NovaSala { get; set; }
        public int SalaId { get; set; }

        public CriacaoSala()
        {
            TemasIds = new List<int>();
        }
    }

    public class ParticiparSala
    {
        public int SalaId { get; set; }
        public int UsuarioId { get; set; }
    }

    public class SalaPartida
    {
        public int SalaId { get; set; }
        public int CriadorSala { get; set; }
        public List<int> Usuarios { get; set; }
        public int TotalJogadores { get; set; }
        public int JogadoresNaSala { get; set; }
        public List<int> Temas { get; set; }
        public int Nivel { get; set; }
        public bool SalaCheia { get; set; }

        public SalaPartida()
        {
            Usuarios = new List<int>();
            Temas = new List<int>();
            SalaCheia = false;
            JogadoresNaSala = 0;
        }
    }
}