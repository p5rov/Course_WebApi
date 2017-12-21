using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(SecretSanta.WebApi.Startup))]
namespace SecretSanta.WebApi
{
    using Microsoft.Owin.Cors;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
                                                          {
                                                              AllowInsecureHttp = true,
                                                              TokenEndpointPath = new PathString("/token"),
                                                              AccessTokenExpireTimeSpan = TimeSpan.FromHours(4),
                                                              Provider = new OAuthAuthorizationServerProvider(),
                                                          };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}