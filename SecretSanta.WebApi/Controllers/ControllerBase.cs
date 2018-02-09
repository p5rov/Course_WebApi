using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

using Microsoft.AspNet.Identity;

namespace SecretSanta.WebApi.Controllers
{
    public class ControllerBase : ApiController
    {
        public string GetUserName()
        {
            ClaimsPrincipal currentUser = User as ClaimsPrincipal;
            string userName = currentUser?.Claims.Where(p => p.Type == "username").Select(p => p.Value).FirstOrDefault();
            return userName;
        }
    }
}