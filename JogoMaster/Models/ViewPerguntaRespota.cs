﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewPerguntaRespota
    {
        public ViewPergunta pergunta { get; set; }
        public List<ViewResposta> respostas { get; set; }
    }
}