using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

using SecretSanta.Repository.Models;
using SecretSanta.WebApi.AuthorizationAttributes;
using SecretSanta.WebApi.Providers;

[assembly: OwinStartup(typeof(SecretSanta.WebApi.Startup))]
namespace SecretSanta.WebApi
{
    using System.Reflection;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Microsoft.Owin.Cors;
    using SecretSanta.Repository;
    using SecretSanta.Repository.Interfaces;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            RegisterDependancies(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
                                                          {
                                                              AllowInsecureHttp = true,
                                                              TokenEndpointPath = new PathString("/token"),
                                                              AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                                                              Provider = new AuthorizationServerProvider(),
                                                          };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void RegisterDependancies(HttpConfiguration config)
        {
            ContainerBuilder builder = new ContainerBuilder();
            Assembly assembly = typeof(UserIdentity).Assembly;

            builder.RegisterAssemblyTypes(new[] { assembly }).AsImplementedInterfaces();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<GroupService>().As<IGroupService>();
            builder.RegisterType<TokenService>().As<ITokenService>();

            builder.RegisterType<Repository<TokenData>>().As<IRepository<TokenData>>();
            builder.RegisterType<Repository<Group>>().As<IRepository<Group>>();
            builder.RegisterType<Repository<GroupParticipant>>().As<IRepository<GroupParticipant>>();
            builder.RegisterType<Repository<UserInvitation>>().As<IRepository<UserInvitation>>();
            builder.RegisterType<Repository<UserIdentity>>().As<IRepository<UserIdentity>>();
            builder.RegisterType<SecretSantaContext>().As<DbContext>().SingleInstance();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UserAuthorizeAttribute>().PropertiesAutowired();

            builder.RegisterWebApiFilterProvider(config);
            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        
    }
}