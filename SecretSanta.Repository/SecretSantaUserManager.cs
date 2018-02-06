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
    public class SecretSantaUserManager : UserManager<UserIdentity>
    {
        public SecretSantaUserManager(IUserStore<UserIdentity> store)
            : base(store)
        {
        }

        public static SecretSantaUserManager GetInstance()
        {
            return new SecretSantaUserManager(new UserStore<UserIdentity>(new SecretSantaContext()));
        }
    }
}
