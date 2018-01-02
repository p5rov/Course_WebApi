using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.WebApi.Providers
{
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.OAuth;

    using SecretSanta.Repository;
    using SecretSanta.Repository.Interfaces;

    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            using (IUserRepository userRepository = new UserRepository())
            {
                IdentityUser user = await userRepository.FindUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("AuthenticationError", "User name or pasword incorrect");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);

        }
    }
}