using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;

using SecretSanta.Repository;
using SecretSanta.WebApi.Models;

namespace SecretSanta.WebApi.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserRepository userRepository = new UserRepository())
            {
                IdentityUser user = await userRepository.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("error", "Invalid user name or password");
                    return;
                }
            }
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            context.Validated(identity);
        }
    }
}