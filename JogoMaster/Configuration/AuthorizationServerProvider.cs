using JogoMaster.Util;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace JogoMaster.Configuration
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var md5Senha = Criptografia.MD5Hash(context.Password);

            using (var ctx = new JogoMasterEntities())
            {
                var usuario = ctx.Usuarios
                    .FirstOrDefault(x => x.Email == context.UserName && x.Senha == md5Senha);

                if (usuario != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("id", usuario.Id.ToString()));                    
                    identity.AddClaim(new Claim("sub", context.UserName));
                    identity.AddClaim(new Claim("role", "user"));

                    context.Validated(identity);
                }
            }

        }
    }
}