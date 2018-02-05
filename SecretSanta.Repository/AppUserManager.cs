using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using SecretSanta.Repository.Models;
using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository
{
    public class AppUserManager : UserManager<UserIdentity>
    {
        public AppUserManager(IUserStore<UserIdentity> store)
            : base(store)
        {
        }

        public static AppUserManager GetInstance()
        {
            return new AppUserManager(new UserStore<UserIdentity>(new AppContext()));
        }
    }
}
