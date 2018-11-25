using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogoMaster.Models
{
    public class ViewUsuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Skin { get; set; }
        public int Pontos { get; set; }
        public bool Cadastrado { get; set; }
        public int IdClassificacao { get; set; }
    }

    public class ViewLogin
    {
        public string Username { get; set; }
        public string Senha { get; set; }
    }
}