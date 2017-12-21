using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("DefaultConnection")
        {
        }

    }
}
