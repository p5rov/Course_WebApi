using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GroupAdminUserId { get; set; }

        public virtual UserIdentity GroupAdminUser { get; set; }

        public virtual List<GroupParticipant> GroupParticipants { get; set; }
    }
}
