using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

using SecretSanta.Repository.Models;
using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository
{
    public class AppContext : IdentityDbContext<UserIdentity>
    {
        public AppContext() : base("DefaultConnection")
        {
        }
    }
}
