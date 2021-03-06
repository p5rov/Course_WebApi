﻿using System;
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
            using (SecretSantaUserManager userManager = SecretSantaUserManager.GetInstance())
            {
                IdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("error", "Invalid user name or password");
                    return;
                }
            }
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            context.Validated(identity);
        }
    }
}