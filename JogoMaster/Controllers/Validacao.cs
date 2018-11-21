using JogoMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace JogoMaster.Controllers
{
    public class Validacao : ApiController
    {
        private JogoMasterEntities ctx;

        public void Refute(bool condition, string message = null)
        {
            if (condition) throw new Exception(message ?? "Condition failed.");
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}