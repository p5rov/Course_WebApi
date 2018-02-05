using System.Collections.Generic;

using SecretSanta.Repository.Models;

namespace SecretSanta.Repository.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SecretSanta.Repository.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SecretSanta.Repository.AppContext";
        }

        protected override void Seed(SecretSanta.Repository.AppContext context)
        {
            //  This method will be called after migrating to the latest version.
            
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
