﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewTema
    {
        public int Id { get; set; }
        public string Tema { get; set; }
        public string Icone { get; set; }
        public string Cor { get; set; }
    }

    public class ListaIds
    {
        public List<int> ids { get; set; }

        public ListaIds() {

            ids = new List<int>();
        }
    }

}