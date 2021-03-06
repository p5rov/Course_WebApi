﻿using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

using Microsoft.AspNet.Identity;

using SecretSanta.Repository.Interfaces;

namespace SecretSanta.WebApi.AuthorizationAttributes
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public ITokenService TokenService { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null && actionContext.Request.Headers.Authorization.Scheme == "Bearer"
                                                                    && HttpContext.Current.User != null)
            {
                string token = actionContext.Request.Headers.Authorization.Parameter;
                ClaimsPrincipal currentUser = HttpContext.Current.User as ClaimsPrincipal;
                string userName = currentUser?.Claims.Where(p => p.Type == "username").Select(p => p.Value).FirstOrDefault();
                if (TokenService.IsValidToken(userName, token))
                {
                    TokenService.ExtendTokenPeriod(userName, token);
                    base.OnAuthorization(actionContext);
                    return;
                }

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Expired token");
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}