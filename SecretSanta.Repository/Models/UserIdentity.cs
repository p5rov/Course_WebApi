using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;

namespace SecretSanta.Repository.Models
{
    public class UserIdentity : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
