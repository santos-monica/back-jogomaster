using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewPerguntaResposta
    {
        public ViewPergunta pergunta { get; set; }
        public List<ViewResposta> respostas { get; set; }
    }

    public class ListViewPerguntaResposta
    {
        public List<ViewPerguntaResposta> lista { get; set; }
        public ListViewPerguntaResposta()
        {
            lista = new List<ViewPerguntaResposta>();
        }
    }

    public class ConsultaNivelTemas
    {
        public int idNivel { get; set; }
        public List<int> idsTema { get; set; }
    }
}