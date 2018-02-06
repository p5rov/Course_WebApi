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
    public class SecretSantaContext : IdentityDbContext<UserIdentity>
    {
        public SecretSantaContext() : base("DefaultConnection")
        {
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupParticipant> GroupParticipants { get; set; }

        public DbSet<UserInvitation> UserInvitations { get; set; }
    }
}
